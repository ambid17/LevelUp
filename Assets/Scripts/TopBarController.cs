using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarController : MonoBehaviour
{
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject fightPanel;
    [SerializeField] private GameObject perksPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject gemsPanel;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button fightButton;
    [SerializeField] private Button perksButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button gemsButton;

    public enum TopBarPanel
    {
        Home,
        Fight,
        Perks,
        Settings,
        Stats,
        Gems
    }
    void Start()
    {
        SetupButtons();
        ShowPanel(TopBarPanel.Home);
    }

    void Update()
    {
        
    }

    private void SetupButtons()
    {
        homeButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Home));
        fightButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Fight));
        perksButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Perks));
        settingsButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Settings));
        statsButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Stats));
        gemsButton.onClick.AddListener(() => ShowPanel(TopBarPanel.Gems));
    }

    public void ShowPanel(TopBarPanel panel)
    {
        homePanel.SetActive(panel == TopBarPanel.Home);
        fightPanel.SetActive(panel == TopBarPanel.Fight);
        perksPanel.SetActive(panel == TopBarPanel.Perks);
        settingsPanel.SetActive(panel == TopBarPanel.Settings);
        statsPanel.SetActive(panel == TopBarPanel.Stats);
        gemsPanel.SetActive(panel == TopBarPanel.Gems);
    }
}
