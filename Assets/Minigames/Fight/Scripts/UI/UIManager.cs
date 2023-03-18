using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public enum UIPanelType
    {
        Effects,
        Pause,
        Reward,
        None
    }
    
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIPanel effectPanel;
        [SerializeField] private UIPanel pausePanel;
        [SerializeField] private UIPanel rewardPanel;
        
        public bool isPaused;

        private UIPanel currentPanel;
        private UIPanelType currentPanelType;
        

        void Start()
        {
            // Make sure all UI is toggled off
            ToggleUiPanel(UIPanelType.Effects, false);
            ToggleUiPanel(UIPanelType.Pause, false);
            ToggleUiPanel(UIPanelType.Reward, false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleUiPanel(UIPanelType.Effects, true);
            }
        
            // Close the current panel
            // If no panels are open, open the pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentPanelType == UIPanelType.None)
                {
                    ToggleUiPanel(UIPanelType.Pause, true);
                }
                // Don't allow closing the reward panel
                else if (currentPanelType == UIPanelType.Effects || currentPanelType == UIPanelType.Pause)
                {
                    Time.timeScale = 1;
                    isPaused = false;
                    currentPanel.Toggle(false);
                }
            }
        }

        public void ToggleUiPanel(UIPanelType panelType, bool isActive)
        {
            Time.timeScale = isActive ? 0 : 1;
            isPaused = isActive;
            
            switch (panelType)
            {
                case UIPanelType.Effects:
                    effectPanel.Toggle(isActive);
                    break;
                case UIPanelType.Pause:
                    pausePanel.Toggle(isActive);
                    break;
                case UIPanelType.Reward:
                    rewardPanel.Toggle(isActive);
                    break;
            }
        }
    }
}
