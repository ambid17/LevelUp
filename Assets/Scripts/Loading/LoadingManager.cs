using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Minigames.Fight;
using System.ComponentModel;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Platform.EventService.AddPermanent<SceneIsReadyEvent>(SetSceneReady);
        SceneManager.LoadScene("Fight");
    }

    private void SetSceneReady(SceneIsReadyEvent e)
    {
        Platform.EventService.RemovePermanent<SceneIsReadyEvent>(SetSceneReady);
        Destroy(gameObject);
    }
}
