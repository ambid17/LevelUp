using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;

    [SerializeField] private FightDataLoader _fightDataLoader;

    private bool isDataLoaded;
    
    void Start()
    {
        _fightDataLoader.onDataLoaded.AddListener(DataLoaderCompleted);

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
                _fightDataLoader.Load();
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
