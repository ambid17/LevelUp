using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private TMP_Text worldNameText;
    [SerializeField] private Image worldImage;
    [SerializeField] private Slider areaProgressSlider;
    [SerializeField] private Button previousCountryButton;
    [SerializeField] private Button nextCountryButton;
    
    void Start()
    {
        previousCountryButton.onClick.AddListener(PreviousCountry);
        previousCountryButton.interactable =
            GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.Index > 0;
        
        nextCountryButton.onClick.AddListener(NextCountry);
        nextCountryButton.interactable =
            GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.IsConquered;
        
        GameManager.GameStateManager.enemyKilled.AddListener(OnEnemyKilled);
        
        SetWorld();
    }

    void Update()
    {
        
    }

    public void SetWorld()
    {
        World world = GameManager.SettingsManager.progressSettings.CurrentWorld;
        worldNameText.text = world.Name;
        worldImage.sprite = world.WorldSprite;
    }

    private void OnEnemyKilled()
    {
        float percentComplete = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.ConquerPercent;
        areaProgressSlider.value = percentComplete;

        if (percentComplete >= 1)
        {
            nextCountryButton.interactable = true;
        }
        // TODO update world sprite
    }

    private void PreviousCountry()
    {
        GameManager.SettingsManager.progressSettings.CurrentWorld.TrySetPreviousCountry();
    }

    private void NextCountry()
    {
        GameManager.SettingsManager.progressSettings.CurrentWorld.TrySetNextCountry();
    }
}
