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
        [SerializeField] private EffectItem effectItemPrefab;

        [SerializeField] private Transform treeContainer;
        [SerializeField] private Transform tree;
        [SerializeField] private EffectInspector inspector;

        [SerializeField] private Button closeButton;

        private EffectTree _effectTree;

        private Camera _mainCamera;
        private bool _isDragging;
        private Vector3 _lastMousePos;
        void Start()
        {
            _mainCamera = Camera.main;
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
                //_lastMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
                tree.transform.localScale += new Vector3(Input.mouseScrollDelta.y, Input.mouseScrollDelta.y) * 0.5f;
            }
        }

        private void OnLayoutItemSelected(EffectItemSelectedEvent e)
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