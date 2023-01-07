using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private ProgressSettings _progressSettings;
    [SerializeField] private WorldButton _buttonPrefab;
    [SerializeField] private Transform _worldContainer;
    [SerializeField] private WorldInspector _worldInspector;
    [SerializeField] private FightDataLoader _fightDataLoader;
    private Camera _camera;
    
    void Start()
    {
        _camera = Camera.main;;
        _fightDataLoader.Load();
        CreatePlanetButtons();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                WorldButton button = hit.transform.GetComponent<WorldButton>();
                InspectWorld(button.world);
            }
        }
    }

    private void CreatePlanetButtons()
    {
        foreach (var world in _progressSettings.Worlds)
        {
            WorldButton worldButton = Instantiate(_buttonPrefab, _worldContainer);
            worldButton.SetForWorld(world);
        }
    }
    
    private void InspectWorld(World world)
    {
        _worldInspector.InspectWorld(world);
    }
}
