using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private WorldButton _buttonPrefab;
    [SerializeField] private Transform _worldContainer;
    [SerializeField] private WorldInspector _worldInspector;
    [SerializeField] private SunInspector _sunInspector;
    private Camera _camera;
    
    void Start()
    {
        _camera = Camera.main;;
        CreatePlanetButtons();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                WorldButton button = hit.transform.GetComponent<WorldButton>();

                if (button != null)
                {
                    _worldInspector.InspectWorld(button.world);
                    _sunInspector.Hide();
                }

                SunButton sunButton = hit.transform.GetComponent<SunButton>();

                if (sunButton != null)
                {
                    _sunInspector.InspectSun();
                    _worldInspector.Hide();
                }
            }
            else
            {
                _sunInspector.Hide();
                _worldInspector.Hide();
            }
        }
    }

    private void CreatePlanetButtons()
    {
        foreach (var world in Platform.ProgressSettings.Biomes)
        {
            WorldButton worldButton = Instantiate(_buttonPrefab, _worldContainer);
            worldButton.SetForWorld(world);
        }
    }
}
