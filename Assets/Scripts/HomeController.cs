using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;
    [SerializeField] private WorldButton buttonPrefab;
    
    void Start()
    {
        CreatePlanetButtons();
    }

    private void CreatePlanetButtons()
    {
        foreach (var world in _progressSettings.Worlds)
        {
            WorldButton worldButton = Instantiate(buttonPrefab, transform);
            worldButton.SetForWorld(world);
        }
    }
}
