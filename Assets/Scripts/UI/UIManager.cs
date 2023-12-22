using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public enum UIPanelType
    {
        None,
        Effect,
        Pause,
        DevCheats
    }
    
    /// <summary>
    /// The UIManager exists to be able to disable any UI with escape, and to be able to open the pause menu based on the state of other UI
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public ResourceTypeSpriteDictionary ResourceSpriteDictionary;

        [SerializeField] private UIPanel upgradePanel;
        [SerializeField] private UIPanel pausePanel;
        [SerializeField] private UIPanel devCheatsPanel;
        
        public bool isPaused;

        [Header("Set at runtime")]
        [SerializeField] private UIPanelType currentPanelType;
        

        void Start()
        {
            // Make sure all UI is toggled off
            ToggleUiPanel(UIPanelType.Effect, false);
            ToggleUiPanel(UIPanelType.Pause, false);
            ToggleUiPanel(UIPanelType.DevCheats, false);
        }

        void Update()
        {
            // Close the current panel
            // If no panels are open, open the pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentPanelType == UIPanelType.None)
                {
                    ToggleUiPanel(UIPanelType.Pause, true);
                }
                else
                {
                    ToggleUiPanel(currentPanelType, false);
                }
            }
        }

        public void ToggleUiPanel(UIPanelType panelType, bool isActive)
        {
            isPaused = isActive;

            currentPanelType = isActive ? panelType : UIPanelType.None;
            
            switch (panelType)
            {
                case UIPanelType.Effect:
                    upgradePanel.Toggle(isActive);
                    break;
                case UIPanelType.Pause:
                    pausePanel.Toggle(isActive);
                    break;
                case UIPanelType.DevCheats:
                    devCheatsPanel.Toggle(isActive);
                    break;
            }
        }
    }
}
