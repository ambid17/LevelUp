using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EffectUpgradeItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image lineImagePrefab;
        private EffectUpgradeItem _effectItemPrefab;

        public EffectNode effectNode;

        public void Setup(EffectNode node, EffectUpgradeItem prefab)
        {
            effectNode = node;
            _effectItemPrefab = prefab;

            icon.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            gameObject.name = effectNode.Name;

            if (effectNode.Effect == null)
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

            GenerateChildren();
        }

        private void GenerateChildren()
        {
            float angleInterval = 360f / effectNode.Children.Count;
            float radius = 150;
            for (int i = 0; i < effectNode.Children.Count; i++)
            {
                // positioning
                var effectItem = Instantiate(_effectItemPrefab, transform);
                EffectNode newNode = effectNode.Children[i];
                effectItem.Setup(newNode, _effectItemPrefab);
                float angle = angleInterval * i * Mathf.Deg2Rad;
                effectItem.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                // create line
                var image = Instantiate(lineImagePrefab, transform);
                image.transform.position = Vector3.Lerp(transform.position, effectItem.transform.position, 0.5f);
                float width = Vector2.Distance(transform.position, effectItem.transform.position);
                image.rectTransform.sizeDelta = new Vector2(width, 1);
                
                // rotate line
                Vector2 offset = (transform.position - effectItem.transform.position).AsVector2();
                float rotation = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                image.transform.rotation = Quaternion.Euler(0,0,rotation);
            }
        }

        private void SelectLayoutItem()
        {
            GameManager.EventService.Dispatch(new EffectLayoutUiItemSelectedEvent(this));
        }
    }
}