using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;
    [SerializeField] private WorldButton _buttonPrefab;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private WorldInspector _worldInspector;
    [SerializeField] private FightDataLoader _fightDataLoader;
    
    void Start()
    {
        _fightDataLoader.Load();
        CreatePlanetButtons();
    }

    private void CreatePlanetButtons()
    {
        foreach (var world in _progressSettings.Worlds)
        {
            WorldButton worldButton = Instantiate(_buttonPrefab, _buttonContainer);
            worldButton.SetForWorld(() => InspectWorld(world), world);
        }
    }
    
    private void InspectWorld(World world)
    {
        _worldInspector.InspectWorld(world);
    }
}
