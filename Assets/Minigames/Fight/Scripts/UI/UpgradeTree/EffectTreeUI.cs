using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class EffectTreeUI : UIPanel
    {
        [SerializeField] private EffectUpgradeItem effectItemPrefab;

        [SerializeField] private Transform treeContainer;
        [SerializeField] private EffectInspector inspector;

        [SerializeField] private Button closeButton;

        private EffectTree _effectTree;

        void Start()
        {
            closeButton.onClick.AddListener(Close);
            BuildTree();
            GenerateUi();
            GameManager.EventService.Add<EffectUpgradeItemSelectedEvent>(OnLayoutItemSelected);
        }

        private void OnLayoutItemSelected(EffectUpgradeItemSelectedEvent e)
        {
            // TODO: reposition the UI to center on the item?
            // TODO: disable/enable the proper tiers of items - this isn't necessary if they are spaced properly
        }

        private void Close()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Effects, false);
        }

        private void BuildTree()
        {
            _effectTree = new EffectTree();
            
            foreach (var effect in GameManager.SettingsManager.effectSettings.AllEffects)
            {
                _effectTree.Add(effect);
            }
        }

        private void GenerateUi()
        {
            var effectItem = Instantiate(effectItemPrefab, treeContainer);
            effectItem.Setup(_effectTree.RootNode, effectItemPrefab);
        }
    }
}