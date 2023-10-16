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
        EffectUpgrade,
        EffectCreate,
        Pause,
        Reward,
    }
    
    public class UIManager : MonoBehaviour
    {
        public ResourceTypeSpriteDictionary ResourceSpriteDictionary;

        [SerializeField] private UIPanel effectUpgradePanel;
        [SerializeField] private UIPanel effectCreatePanel;
        [SerializeField] private UIPanel pausePanel;
        [SerializeField] private UIPanel rewardPanel;
        
        public bool isPaused;

        [Header("Set at runtime")]
        [SerializeField] private UIPanelType currentPanelType;
        

        void Start()
        {
            // Make sure all UI is toggled off
            ToggleUiPanel(UIPanelType.EffectUpgrade, false);
            ToggleUiPanel(UIPanelType.EffectCreate, false);
            ToggleUiPanel(UIPanelType.Pause, false);
            ToggleUiPanel(UIPanelType.Reward, false);
            GameManager.EventService.Add<PlayerInteractedEvent>(OnPlayerInteract);
        }

        private void OnPlayerInteract(PlayerInteractedEvent e)
        {
            if (e.InteractionType == InteractionType.Create || e.InteractionType == InteractionType.Upgrade)
            {
                ToggleUiPanel(UIPanelType.EffectUpgrade, true);
            }
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
                // Don't allow closing the reward panel
                else
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
                case UIPanelType.EffectUpgrade:
                    effectUpgradePanel.Toggle(isActive);
                    break;
                case UIPanelType.Pause:
                    pausePanel.Toggle(isActive);
                    break;
                case UIPanelType.EffectCreate:
                    effectCreatePanel.Toggle(isActive);
                //    break;
                //case UIPanelType.Reward:
                //    rewardPanel.Toggle(isActive);
                    break;
            }
        }
    }
}
