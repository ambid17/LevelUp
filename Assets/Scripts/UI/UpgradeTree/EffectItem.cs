using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EffectItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image lineImagePrefab;
        [SerializeField] private GameObject container;
        private EffectItem _effectItemPrefab;

        public EffectNode effectNode;

        [SerializeField]
        private List<EffectItem> children;
        [SerializeField]
        private Image lineToParent;
        [SerializeField]
        public EffectItem parent;

        public void Setup(EffectNode node, EffectItem prefab, EffectItem parent = null)
        {
            effectNode = node;
            _effectItemPrefab = prefab;

            icon.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            gameObject.name = effectNode.Name;

            if (effectNode.Effect == null || effectNode.Effect.Icon == null)
            {
                text.gameObject.SetActive(true);
                text.text = effectNode.Name;
            }
            else
            {
                icon.gameObject.SetActive(transform);
                icon.sprite = effectNode.Effect.Icon;
            }
            button.onClick.AddListener(SelectLayoutItem);

            if (parent != null)
            {
                this.parent = parent;
                // create line
                lineToParent = Instantiate(lineImagePrefab, transform);
                lineToParent.transform.position = Vector3.Lerp(transform.position, parent.transform.position, 0.5f);
                float width = Vector2.Distance(transform.position, parent.transform.position);
                lineToParent.rectTransform.sizeDelta = new Vector2(width, 1);
                
                // rotate line
                Vector2 offset = (transform.position - parent.transform.position).AsVector2();
                float rotation = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                lineToParent.transform.rotation = Quaternion.Euler(0,0,rotation);
            }

            GenerateChildren();
        }

        private void GenerateChildren()
        {
            float angleInterval = 360f / effectNode.Children.Count;
            float angle = 315;
            for (int i = 0; i < effectNode.Children.Count; i++)
            {
                // positioning
                var effectItem = Instantiate(_effectItemPrefab, transform);
                EffectNode newNode = effectNode.Children[i];
                effectItem.transform.localPosition = new Vector2(150, 0).Rotate(angle);

                effectItem.Setup(newNode, _effectItemPrefab, this);
                children.Add(effectItem);

                angle = (angle + angleInterval) % 360;
            }
        }

        private void SelectLayoutItem()
        {
            if (parent == null)
            {
                return;
            }
            if (effectNode.TierCategory != TierCategory.None)
            {
                Platform.EventService.Dispatch(new UnlockItemSelectedEvent(this));
            }
            else if (effectNode.Effect != null)
            {
                Platform.EventService.Dispatch(new EffectItemSelectedEvent(this));
            }
            ToggleChildren(true);
            if (children.Count > 0)
            {
                parent.ToggleChildrenExcept(false, this);
            }
        }

        public void ToggleChildren(bool shouldBeActive)
        {
            foreach (var child in children)
            {
                child.Toggle(shouldBeActive);
            }
        }

        public void ToggleChildrenExcept(bool shouldBeActive, EffectItem itemToIgnore)
        {
            Toggle(shouldBeActive);
            
            foreach (var child in children)
            {
                if (child == itemToIgnore)
                {
                    continue;
                }
                child.Toggle(shouldBeActive);
            }
        }

        public void Toggle(bool shouldBeActive)
        {
            container.SetActive(shouldBeActive);
            if (lineToParent != null)
            {
                lineToParent.gameObject.SetActive(shouldBeActive);
            }
        }
        
        public void SetupRoot()
        {
            foreach (var child in children)
            {
                child.ToggleChildrenRecursively(false);
            }
        }

        public void ToggleChildrenRecursively(bool shouldBeActive)
        {
            foreach (var child in children)
            {
                child.Toggle(shouldBeActive);
                child.ToggleChildrenRecursively(shouldBeActive);
            }
        }

        public override string ToString()
        {
            return effectNode.Name;
        }
    }
}