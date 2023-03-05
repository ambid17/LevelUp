using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class EffectTreeUI : MonoBehaviour
    {
        [SerializeField] private EffectUpgradeItem effectItemPrefab;
        [SerializeField] private EffectParentItem effectParentItemPrefab;

        [SerializeField] private Transform treeContainer;
        [SerializeField] private EffectInspector inspector;
        [SerializeField] private GameObject container;

        [SerializeField] private Button closeButton;

        private EffectTree _effectTree;

        public bool IsActive => container.activeInHierarchy;
    
        void Start()
        {
            closeButton.onClick.AddListener(Close);
            BuildTree();
            GameManager.EventService.Add<EffectLayoutUiItemSelectedEvent>(OnLayoutItemSelected);
            //ToggleActive(false);
        }

        private void OnLayoutItemSelected(EffectLayoutUiItemSelectedEvent e)
        {
        }

        public void ToggleActive(bool shouldBeActive)
        {
            container.SetActive(shouldBeActive);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void BuildTree()
        {
            _effectTree = new EffectTree();
            
            foreach (var effect in GameManager.SettingsManager.effectSettings.AllEffects)
            {
                _effectTree.Add(effect);
                // var effectItem = Instantiate(effectItemPrefab, treeContainer);
                // effectItem.Setup(effect);
            }
        }
    
    }
}