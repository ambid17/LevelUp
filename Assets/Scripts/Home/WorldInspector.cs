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
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject loadingTransition;
    
    private Biome _biome;
    void Start()
    {
        Hide();
    }

    public void InspectWorld(Biome biome)
    {
        if (biome == null)
        {
            container.SetActive(false);
            return;
        }
        container.SetActive(true);
        _biome = biome;
        planetNameText.text = biome.Name;
        planetImage.sprite = biome.BiomeSprite;
        progressText.text = $"Conquered";
        conquerButton.onClick.AddListener(ConquerPlanet);
        conquerButton.interactable = biome.IsUnlocked;
    }

    public void Hide()
    {
        container.SetActive(false);

    }

    private void ConquerPlanet()
    {
        GameManager.IsLoadingScene = true;
        Platform.ProgressSettings.CurrentBiome = _biome;
        Instantiate(loadingTransition);
    }
}
