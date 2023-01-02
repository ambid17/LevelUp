using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public static string FIGHT_SCENE = "Fight";
    public Button craftButton;
    
    void Start()
    {
        craftButton.onClick.AddListener(() => LoadGame(FIGHT_SCENE));    
    }

    private void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
