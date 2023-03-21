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
        Effects,
        Pause,
        Reward,
    }
    
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIPanel effectPanel;
        [SerializeField] private UIPanel pausePanel;
        [SerializeField] private UIPanel rewardPanel;
        
        public bool isPaused;

        [Header("Set at runtime")]
        [SerializeField] private UIPanelType currentPanelType;
        

        void Start()
        {
            // Make sure all UI is toggled off
            ToggleUiPanel(UIPanelType.Effects, false);
            ToggleUiPanel(UIPanelType.Pause, false);
            ToggleUiPanel(UIPanelType.Reward, false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && currentPanelType == UIPanelType.None)
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
                    ToggleUiPanel(currentPanelType, false);
                }
            }
        }

        public void ToggleUiPanel(UIPanelType panelType, bool isActive)
        {
            Time.timeScale = isActive ? 0 : 1;
            isPaused = isActive;

             currentPanelType = isActive ? panelType : UIPanelType.None;
            
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
