using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class PauseUI : UIPanel
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button devCheatsButton;
    
        void Start()
        {
            resumeButton.onClick.AddListener(Resume);
            homeButton.onClick.AddListener(Home);
            deleteButton.onClick.AddListener(DeleteSave);
            quitButton.onClick.AddListener(Quit);
            devCheatsButton.onClick.AddListener(DevCheats);
        }

        private void Resume()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Pause, false);
        }

        public void Home()
        {
            SceneManager.LoadScene(0);
        }

        public void DeleteSave()
        {
            try
            {
                var paths = Directory.EnumerateFiles(Application.persistentDataPath);
                foreach (var path in paths)
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"could not delete save data: {e.Message}\n{e.StackTrace}");
            }
            

            Platform.ShouldSave = false;
            Application.Quit();
        }
        
        public void Quit()
        {
            Application.Quit();
        }


        public void DevCheats()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Pause, false);
            GameManager.UIManager.ToggleUiPanel(UIPanelType.DevCheats, true);
        }
    }
}
