using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace NMEditor {
    // Workflow tool providing quick access to game scenes
    public class OpenGameSceneWindow : EditorWindow {
        [MenuItem("Caos Creations/Open Game Scene _F1")]
        private static void Init() {
            // Support docked and floating window behavior
            if(HasOpenInstances<OpenGameSceneWindow>()) {
                OpenGameSceneWindow window = GetWindow<OpenGameSceneWindow>();
                // docked: focus window
                if(window.docked) {
                    window.Focus();
                } else {
                    // floating window: pressing the key toggles it
                    window.Close();
                }
            } else {
                GetWindow<OpenGameSceneWindow>();
            }
        }

        public class GameScenes {
            public string gameID;
            public string displayName;
            public List<EditorBuildSettingsScene> scenes;

            public GameScenes(string gameID, List<EditorBuildSettingsScene> scenes) {
                this.gameID = gameID;
                this.scenes = scenes;
            }

            public void UpdateDisplayName() {
                displayName = gameID.Replace("_", "\n") + $"\n({scenes.Count})";
            }
        }

        private Dictionary<string, GameScenes> sceneLookup;
        private Vector2 scrollPosition;

        private ScenePopup scenePopup;

        private const string HUB_PATH = "Assets/Scenes/";
        private const string GAMES_PATH = "Assets/Games/";
        private const string HUB_ID = "MIGHTIER";
        private const string LAVAQUEST_ID = "LAVAQUEST";

        private void OnEnable() {
            this.titleContent = new GUIContent("Open Game Scene");
            this.minSize = new Vector2(300, 200);
                
            sceneLookup = new Dictionary<string, GameScenes>();
            
            // search build settings to find game scenes
            foreach (var scene in EditorBuildSettings.scenes) {
                var scenePath = scene.path;
                if (string.IsNullOrEmpty(scenePath)) {
                    continue;
                }

                string gameID = "";
                
                if (scenePath.Contains(HUB_PATH)) {
                    gameID = HUB_ID;
                    
                    if(scenePath.Contains("LavaQuest.unity") || scenePath.Contains("InteractiveSkills")) {
                        gameID = LAVAQUEST_ID;
                    }
                } else if (scenePath.Contains(GAMES_PATH)) {
                    // Assets/Games/ROCAT_JUMPUR/
                    // Assets/Games/{gameID}/
                    int start = scenePath.IndexOf(GAMES_PATH) + GAMES_PATH.Length;
                    int end = scenePath.IndexOf("/", start);

                    gameID = scenePath.Substring(start, (end-start));
                }

                if(string.IsNullOrEmpty(gameID)) {
                    continue;
                }
                
                // store scenes
                if(sceneLookup.ContainsKey(gameID)) {
                    sceneLookup[gameID].scenes.Add(scene);
                } else {
                    sceneLookup[gameID] = new GameScenes(gameID, new List<EditorBuildSettingsScene> {scene});
                }
            }
            
            { // reorder the games alphabetically, but put the Hub first
                
                GameScenes hub = sceneLookup[HUB_ID];
                GameScenes LQ = sceneLookup[LAVAQUEST_ID];
                sceneLookup.Remove(LAVAQUEST_ID);
                sceneLookup.Remove(HUB_ID);
                
                // kinda ugly, and dictionary isn't really ordered
                IEnumerable<KeyValuePair<string, GameScenes>> ordered = sceneLookup.OrderBy(kvp => kvp.Key);
                ordered = ordered.Prepend(new KeyValuePair<string, GameScenes> (LAVAQUEST_ID, LQ));
                ordered = ordered.Prepend(new KeyValuePair<string, GameScenes> (HUB_ID, hub));
                
                sceneLookup = ordered.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            // set all display names
            foreach(var scene in sceneLookup) {
                scene.Value.UpdateDisplayName();
            }

            scenePopup = new ScenePopup();
            scenePopup.parentWindow = this;
        }

        private void OnGUI() {
            EditorGUILayout.LabelField("Click to launch first game scene. Right click for additional scenes.");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            int column = 0;
            int row = 0;

            const int WIDTH = 100;
            const int HEIGHT = 100;

            int maxColumns = Mathf.FloorToInt(position.width / WIDTH);
            
            // draw grid of buttons
            foreach (KeyValuePair<string, GameScenes> scenePair in sceneLookup) {

                if(column >= maxColumns) {
                    column = 0;
                    row++;
                }

                int x = column * WIDTH;
                int y = row * HEIGHT;
                Rect rect = new Rect(x, y, WIDTH, HEIGHT);

                if(GUI.Button(rect, scenePair.Value.displayName)) { // button is clicked (can be left or right click)
                    if(Event.current.button == 1) { // detect right click
                        scenePopup.scenes = scenePair.Value;
                        
                        Rect popupRect = new Rect();
                        popupRect.x = Event.current.mousePosition.x;
                        popupRect.y = Event.current.mousePosition.y - rect.height;
                        popupRect.width = rect.width;
                        popupRect.height = rect.height;
                        
                        // draw right click menu
                        PopupWindow.Show(popupRect, scenePopup);
                        break;
                    }

                    // left click opens first scene    
                    OpenScene(scenePair.Key, 0);
                }

                column++;
            }

            GUILayout.EndScrollView();
        }

        public void OpenScene(string gameID, int index) {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(sceneLookup[gameID].scenes[index].path, OpenSceneMode.Single);

                if(!docked) { // close floating window on click
                    Close(); 
                }
            }
        }
        
        // draw the right click popup menu
        public class ScenePopup : PopupWindowContent {
            public OpenGameSceneWindow parentWindow;
            public GameScenes scenes;

            string[] scenePaths;
            string[] sceneNames;
            Vector2 scrollPos;

            // display scrollable list of scenes by name
            public override void OnGUI(Rect rect) {
                scrollPos = GUILayout.BeginScrollView(scrollPos);

                for(int i = 0; i < scenePaths.Length; i++) {
                    if(GUILayout.Button(sceneNames[i])) {
                        parentWindow.OpenScene(scenes.gameID, i);
                    }
                }

                GUILayout.EndScrollView();
            }

            public override void OnOpen() {
                // grab scene paths
                scenePaths = scenes.scenes.Select(s => s.path).ToArray();
                sceneNames = new string[scenePaths.Length];

                for(int i = 0; i < scenePaths.Length; i++) {
                    // Assets/Hub/sceneName.unity
                    // everything to the right of the last '/'
                    // is the scene name
                    
                    int last = scenePaths[i].LastIndexOf("/") + 1;
                    string sceneName = scenePaths[i].Substring(last, (scenePaths[i].Length - last));
                    sceneNames[i] = sceneName;
                }
            }
        }
    }
}
