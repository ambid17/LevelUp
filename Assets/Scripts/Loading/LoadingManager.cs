using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;
    void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        switch (_progressSettings.CurrentWorld.WorldType)
        {
            case WorldType.Fighting:
                // serialize from game manager
                break;
        }

        SceneManager.LoadScene(_progressSettings.CurrentWorld.SceneIndex);
    }
}
