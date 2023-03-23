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

        private List<EditorBuildSettingsScene> scenes;
        private Vector2 scrollPosition;

        private void OnEnable() {
            titleContent = new GUIContent("Open Game Scene");
            minSize = new Vector2(300, 200);
                
            scenes = new List<EditorBuildSettingsScene>();
            
            // search build settings to find game scenes
            foreach (var scene in EditorBuildSettings.scenes) {
                var scenePath = scene.path;
                if (string.IsNullOrEmpty(scenePath)) {
                    continue;
                }
                scenes.Add(scene);
            }
        }

        private void OnGUI() {
            EditorGUILayout.LabelField("Click to launch scene.");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            int column = 0;
            int row = 0;

            const int WIDTH = 100;
            const int HEIGHT = 100;

            int maxColumns = Mathf.FloorToInt(position.width / WIDTH);
            
            // draw grid of buttons
            foreach (var scene in scenes) {

                if(column >= maxColumns) {
                    column = 0;
                    row++;
                }

                int x = column * WIDTH;
                int y = row * HEIGHT;
                Rect rect = new Rect(x, y, WIDTH, HEIGHT);

                string sceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                if(GUI.Button(rect, sceneName)) { 
                    OpenScene(scene.path);
                }

                column++;
            }

            GUILayout.EndScrollView();
        }

        public void OpenScene(string path) {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                if(!docked) { // close floating window on click
                    Close(); 
                }
            }
        }
    }
}
