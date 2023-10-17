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
        // TODO don't let this be true until the rooms are generated. Set via a event
        isDataLoaded = true;
        return;
    }

    private IEnumerator LoadScene()
    {
        // make sure to wait at least 1 second so the loading screen doesn't just flicker
        while (!isDataLoaded)
        {
            yield return new WaitForSeconds(2);
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Fight");
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
    
    
}
