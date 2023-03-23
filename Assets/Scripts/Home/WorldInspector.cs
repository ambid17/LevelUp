using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldInspector : MonoBehaviour
{
    [SerializeField] private TMP_Text planetNameText;
    [SerializeField] private Image planetImage;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Button conquerButton;
    [SerializeField] private Button travelButton;
    [SerializeField] private GameObject container;
    
    private const string loadingSceneName = "Loading";

    private World _world;
    void Start()
    {
        Hide();
    }

    public void InspectWorld(World world)
    {
        if (world == null)
        {
            container.SetActive(false);
            return;
        }
        container.SetActive(true);
        _world = world;
        planetNameText.text = world.Name;
        planetImage.sprite = world.WorldSprite;
        progressText.text = $"Conquered {world.GetConqueredCountryCount()}/{world.Countries.Count} countries";
        conquerButton.onClick.AddListener(ConquerPlanet);
        travelButton.onClick.AddListener(TravelToPlanet);

        conquerButton.interactable = world.IsUnlocked;
        travelButton.interactable = world.IsConquered();
    }

    public void Hide()
    {
        container.SetActive(false);

    }
    
    private void ConquerPlanet()
    {
        GameManager.IsLoadingScene = true;
        GameManager.ProgressSettings.CurrentWorld = _world;
        GameManager.ProgressSettings.CurrentWorld.IsFighting = true;
        SceneManager.LoadScene(loadingSceneName);
    }
    
    private void TravelToPlanet()
    {
        GameManager.IsLoadingScene = true;
        GameManager.ProgressSettings.CurrentWorld = _world;
        GameManager.ProgressSettings.CurrentWorld.IsFighting = false;
        SceneManager.LoadScene(loadingSceneName);
    }

    
}
