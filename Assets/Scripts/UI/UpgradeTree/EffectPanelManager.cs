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
        }

        [SerializeField] private UIPanel metaPanel;
        [SerializeField] private UIPanel upgradePanel;
        
        [SerializeField] private Button metaPanelButton;
        [SerializeField] private Button upgradePanelButton;

        void Start()
        {
            metaPanelButton.onClick.AddListener(() => ToggleUiPanel(EffectPanelType.Meta));
            upgradePanelButton.onClick.AddListener(() => ToggleUiPanel(EffectPanelType.Upgrade));
        }

        public void ToggleUiPanel(EffectPanelType panelType)
        {
            metaPanel.Toggle(panelType == EffectPanelType.Meta);
            upgradePanel.Toggle(panelType == EffectPanelType.Upgrade);
        }
    }
}