using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Minigames.Fight;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;

    private bool isDataLoaded;
    
    void Start()
    {
        StartCoroutine(LoadScene());
        LoadData();
    }

    private void LoadData()
    {
        if (_progressSettings.CurrentWorld.IsFighting)
        {
            isDataLoaded = true;
            return;
        }
        
        // TODO: Load data for each game
        // switch (_progressSettings.CurrentWorld.WorldType)
        // {
        //     case WorldType.Crafting:
        //         _gameDataLoader.Load();
        //         isDataLoaded = true;
        //         break;
        // }
    }

    private IEnumerator LoadScene()
    {
        // make sure to wait at least 1 second so the loading screen doesn't just flicker
        while (!isDataLoaded)
        {
            yield return new WaitForSeconds(2);
        }

        if (_progressSettings.CurrentWorld.IsFighting)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Fight", LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(_progressSettings.CurrentWorld.SkillingSceneIndex);
        }
    }
    
    
}
