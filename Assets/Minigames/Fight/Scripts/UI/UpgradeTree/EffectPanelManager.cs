using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class EffectPanelManager : MonoBehaviour
    {
        public enum EffectPanelType
        {
            Meta,
            Upgrade,
            Unlock,
        }

        [SerializeField] private UIPanel metaPanel;
        [SerializeField] private UIPanel upgradePanel;
        [SerializeField] private UIPanel unlockPanel;
        
        [SerializeField] private Button metaPanelButton;
        [SerializeField] private Button upgradePanelButton;
        [SerializeField] private Button unlockPanelButton;

        void Start()
        {
            metaPanelButton.onClick.AddListener(() => ToggleUiPanel(EffectPanelType.Meta));
            upgradePanelButton.onClick.AddListener(() => ToggleUiPanel(EffectPanelType.Upgrade));
            unlockPanelButton.onClick.AddListener(() => ToggleUiPanel(EffectPanelType.Unlock));
        }

        public void ToggleUiPanel(EffectPanelType panelType)
        {
            metaPanel.Toggle(panelType == EffectPanelType.Meta);
            upgradePanel.Toggle(panelType == EffectPanelType.Upgrade);
            unlockPanel.Toggle(panelType == EffectPanelType.Unlock);
        }
    }
}