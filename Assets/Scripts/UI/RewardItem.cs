using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class RewardItem : MonoBehaviour
    {
        [SerializeField] private Image isSelectedImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image rewardImage;
        [SerializeField] private Button selectButton;

        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Sprite selectedSprite;

        private Effect _myEffect;
        private EventService _eventService;

        private void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<EffectSelectedEvent>(OnSelected);
        }

        public void Setup(Effect effect, Action<Effect> callback)
        {
            _myEffect = effect;
            nameText.text = effect.Name;
            descriptionText.text = effect.GetDescription();
            rewardImage.sprite = effect.Icon;
            selectButton.onClick.AddListener(() => callback(_myEffect));
        }

        private void OnSelected(EffectSelectedEvent e)
        {
            isSelectedImage.sprite = e.Effect == _myEffect ? selectedSprite : unselectedSprite;
        }
    }
}