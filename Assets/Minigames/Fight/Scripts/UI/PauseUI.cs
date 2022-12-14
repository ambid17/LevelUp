using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button quitButton;
    
        void Start()
        {
            resumeButton.onClick.AddListener(Resume);
            homeButton.onClick.AddListener(Home);
            quitButton.onClick.AddListener(Quit);
        }

        private void Resume()
        {
            gameObject.SetActive(false);
        }

        public void Home()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
