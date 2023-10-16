using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Object = UnityEngine.Object;
using System.Text;

namespace Editor
{
    public class FindReferences : EditorWindow
    {
        private Vector2 scrollPos;
        public Object target;
        public Object[] objectsWithTargetGUID;

        public bool fromSelection;

        private string[] fileTypes = { "prefab", "unity", "asset", "mat" };
        private string[] assetFiles;
        private string totalTime;

        private ReferenceDatabase referenceDatabase;

        [MenuItem("Caos Creations/Find References")]
        public static void Init()
        {
            GetWindow<FindReferences>();
        }

        private void OnGUI()
        {
            GUILayout.Label("Database is built and stored in a local file.\nRebuild when you make significant changes.\nRebuilds take 30 seconds with all the games.");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rebuild DB"))
            {
                var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                referenceDatabase.Create();

                stopWatch.Stop();
                totalTime = stopWatch.Elapsed.ToString();
            }
            else if (GUILayout.Button("Load DB"))
            {
                referenceDatabase.Load();
            }
            GUILayout.EndHorizontal();

            int referenceCount = referenceDatabase.GetCount();
            GUILayout.Label("Database entries: " + referenceCount);
            if (referenceCount == 0)
            {
                EditorGUILayout.HelpBox("Rebuild/Load Required. 0 references in the database.", MessageType.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(totalTime))
            {
                EditorGUILayout.LabelField("DB Creation time:", totalTime);
            }

            GUILayout.Space(20);
            GUILayout.Label("-------");
            GUILayout.Space(20);

            fromSelection = GUILayout.Toggle(fromSelection, "Set Target from Selection");

            if (GUILayout.Button("Find"))
            {
                if (target != null)
                {
                    string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(target));
                    loadReferencesForGUID(guid);
                }
                else
                {
                    Debug.LogError("No Target Object!");
                }
            }

            GUILayout.Label("Target");
            target = EditorGUILayout.ObjectField(target, typeof(Object), false);

            GUILayout.Label("References:");
            if (objectsWithTargetGUID != null)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                EditorGUI.indentLevel++;
                foreach (var objectRef in objectsWithTargetGUID)
                {
                    EditorGUILayout.ObjectField(objectRef, typeof(Object));
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndScrollView();
            }
        }

        // EditorWindow callback
        private void OnSelectionChange()
        {
            if (fromSelection)
            {
                if (Selection.activeObject != target)
                {
                    target = Selection.activeObject;

                    if (target != null)
                    {
                        loadReferencesForGUID(Selection.assetGUIDs[0]);
                    }

                    Repaint();
                }
            }
        }

        private void loadReferencesForGUID(string guid)
        {
            objectsWithTargetGUID = null; // clear out

            List<string> refs = referenceDatabase.GetReferencesTo(guid);
            if (refs != null)
            {
                objectsWithTargetGUID = refs.Select(r => AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(r))).ToArray();
            }
        }

        public IEnumerable<FileInfo> GetAllPossibleFiles()
        {
            var dir = new DirectoryInfo(Application.dataPath);

            foreach (var fileSearchType in fileTypes)
            {
                var assets = dir.GetFiles($"*.{fileSearchType}", SearchOption.AllDirectories);
                foreach (var assetInfo in assets)
                {
                    if (assetInfo.Exists)
                    {
                        yield return assetInfo;
                    }
                }
            }
        }

        private void OnEnable()
        {
            assetFiles = GetAllPossibleFiles().Select(info => info.FullName).ToArray();
            referenceDatabase = new ReferenceDatabase(assetFiles);
        }

        public class ReferenceDatabase
        {
            private string[] files;

            const int GUID_HEADER_LENGTH = 6; // "guid: "
            const int GUID_TEXT_LENGTH = 32; // "0000000000000000e000000000000000"

            // dictionary contents: <guid, referenced by>
            private Dictionary<string, HashSet<string>> lookup;
            private const string FILE_NAME = "/referenceDB.bin";

            public ReferenceDatabase(string[] files)
            {
                this.files = files;

                lookup = new Dictionary<string, HashSet<string>>();
            }

            private string GetPath()
            {
                return Application.persistentDataPath + FILE_NAME;
            }

            public void Create()
            {
                lookup.Clear();

                const int MAX_CHARS = 100 * 1000 * 1000; // needs to be bigger than the biggest file in mightier

                // allocate a buffer to work out of
                char[] buffer = new char[MAX_CHARS];
                StringBuilder sb = new StringBuilder(MAX_CHARS);

                string fileGUID;
                string fileText;
                string toMatch = "guid: ";
                int dataPathLength = Application.dataPath.Length - 6; // -6 for Assets

                for (int i = 0; i < files.Length; i++)
                {
                    // reuse buffer for processing file input (reduces allocations)
                    ReadFileText(ref buffer, out int charsRead, files[i]);
                    sb.Clear();
                    sb.Append(buffer, 0, charsRead);
                    fileText = sb.ToString(); // actual string allocation needed

                    fileGUID = AssetDatabase.AssetPathToGUID(files[i].Substring(dataPathLength));


                    // scan the file for "guid: "
                    int index = fileText.IndexOf(toMatch, StringComparison.Ordinal);
                    while (index >= 0)
                    {
                        string guid = fileText.Substring(index + GUID_HEADER_LENGTH, GUID_TEXT_LENGTH);

                        if (!lookup.ContainsKey(guid))
                        {
                            lookup.Add(guid, new HashSet<string>());
                        }

                        lookup[guid].Add(fileGUID); // add this file as a reference

                        // find next
                        index = fileText.IndexOf(toMatch, index + 1, StringComparison.Ordinal);
                    }
                }

                // write out to file
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, lookup);
                File.WriteAllBytes(GetPath(), stream.ToArray());
            }

            public void Load()
            {
                string path = GetPath();
                if (File.Exists(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // load from the file 
                    byte[] bytes = File.ReadAllBytes(path);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;

                    lookup = (Dictionary<string, HashSet<string>>)formatter.Deserialize(stream);
                }
                else
                {
                    Debug.LogError("FindReferences: File not found");
                }
            }

            public List<string> GetReferencesTo(string guid)
            {
                if (lookup.ContainsKey(guid))
                {
                    return lookup[guid].ToList();
                }
                else
                {
                    return null;
                }
            }

            public int GetCount()
            {
                return lookup.Count;
            }


            // reuse char buffer, report how many chars were read
            private void ReadFileText(ref char[] buffer, out int charsRead, string path)
            {
                using (var sr = new StreamReader(path))
                {
                    charsRead = (int)sr.BaseStream.Length;
                    sr.Read(buffer, 0, charsRead);
                }
            }
        }
    }
}
