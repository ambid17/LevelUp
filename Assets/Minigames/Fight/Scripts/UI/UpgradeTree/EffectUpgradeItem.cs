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
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        public Effect effect;

        public void Setup(Effect effect)
        {
            this.effect = effect;
            
            icon.gameObject.SetActive(true);
            icon.sprite = effect.Icon;
            gameObject.name = effect.Name;
            
            button.onClick.AddListener(SelectLayoutItem);
        }

        private void SelectLayoutItem()
        {
            GameManager.EventService.Dispatch(new EffectLayoutUiItemSelectedEvent(this));
        }
    }
}
