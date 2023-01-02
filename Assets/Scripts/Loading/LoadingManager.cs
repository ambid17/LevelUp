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
        //_gameDataLoader.onDataLoaded.AddListener(DataLoaderCompleted);
        DataLoaderCompleted();
        StartCoroutine(LoadScene());
        LoadData();
    }
    
    private void DataLoaderCompleted()
    {
        isDataLoaded = true;
    }

    private void LoadData()
    {
        switch (_progressSettings.CurrentWorld.WorldType)
        {
            case WorldType.Fighting:
                //_gameDataLoader.Load();
                break;
        }
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
            SceneManager.LoadScene("Fight");
        }
        else
        {
            SceneManager.LoadScene(_progressSettings.CurrentWorld.SkillingSceneIndex);
        }
    }
    
    
}
