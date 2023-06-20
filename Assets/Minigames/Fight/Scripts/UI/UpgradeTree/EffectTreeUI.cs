using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class EffectTreeUI : UIPanel
    {
        [SerializeField] private bool showAllEffects;
        [SerializeField] private EffectItem effectItemPrefab;

        [SerializeField] private Transform treeContainer;
        [SerializeField] private Transform tree;

        [SerializeField] private Button closeButton;

        private EffectTree _effectTree;

        private bool _isDragging;
        private Vector3 _lastMousePos;
        [SerializeField]
        private EffectItem _currentParent;
        
        void Start()
        {
            closeButton.onClick.AddListener(Close);
            BuildTree();
            GenerateUi();
            GameManager.EventService.Add<EffectItemSelectedEvent>(OnLayoutItemSelected);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _lastMousePos = Input.mousePosition;
            }
        
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                Vector3 currentMousePos = Input.mousePosition;
                Vector3 offset = _lastMousePos - currentMousePos;
                tree.transform.position -= offset;
                _lastMousePos = currentMousePos;
            }
            
            if (Input.mouseScrollDelta.magnitude != 0)
            {
                Vector3 newScale = tree.transform.localScale +
                                   new Vector3(Input.mouseScrollDelta.y, Input.mouseScrollDelta.y) * 0.2f;

                // Clamp the zoom to reasonable values
                newScale = newScale.Clamp(Vector3.one * 0.4f, Vector3.one * 2f);
                tree.transform.localScale = newScale;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_currentParent.parent == null)
                {
                    return;
                }
                _currentParent.Toggle(false);
                _currentParent.ToggleChildren(false);
                _currentParent = _currentParent.parent;
                _currentParent.Toggle(true);
                _currentParent.ToggleChildren(true);
                GameManager.EventService.Dispatch(new EffectItemSelectedEvent(_currentParent));
            }
        }

        private void OnLayoutItemSelected(EffectItemSelectedEvent e)
        {
            EffectItem selected = e.EffectItem;

            if (selected == null)
            {
                return;
            }
            _currentParent = selected;
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

            // For debug, allow showing all effects
            var effectsList = GameManager.SettingsManager.effectSettings.AllEffects;
            if (!showAllEffects)
            {
                effectsList = effectsList.Where(e => e.IsUnlocked).ToList();
            }
            
            foreach (var effect in effectsList)
            {
                _effectTree.Add(effect);
            }
        }

        private void GenerateUi()
        {
            var effectItem = Instantiate(effectItemPrefab, treeContainer);
            effectItem.Setup(_effectTree.RootNode, effectItemPrefab);
            effectItem.SetupRoot();
            _currentParent = effectItem;
        }
    }
}