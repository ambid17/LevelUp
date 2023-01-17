using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SunInspector : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button perksButton;
    [SerializeField] private Button gemsButton;

    [SerializeField] private GameObject container;
    
    [SerializeField] private ProgressSettings _progressSettings;

    void Start()
    {
        Hide();
    }

    public void InspectSun()
    {
        container.SetActive(true);

        resetButton.onClick.AddListener(OpenResetMenu);
        perksButton.onClick.AddListener(OpenPerksMenu);
        gemsButton.onClick.AddListener(OpenGemsMenu);

        progressText.text = $"Conquered {_progressSettings.WorldsConquered}/{_progressSettings.Worlds.Count} Worlds";
        resetButton.interactable = _progressSettings.WorldsConquered == _progressSettings.Worlds.Count;
    }

    public void Hide()
    {
        container.SetActive(false);
    }
    
    private void OpenResetMenu()
    {
        
    }
    
    private void OpenPerksMenu()
    {
        
    }
    
    private void OpenGemsMenu()
    {
        
    }
}
