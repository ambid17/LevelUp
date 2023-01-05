using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fish
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _fishImage;
        [SerializeField] private TMP_Text _fishNameText;
        [SerializeField] private TMP_Text _fishCountText;

        private Fish _fish;
        private EventService _eventService;
        private void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<ReeledInEvent>(UpdateItem);
        }

        public void Setup(Fish fish)
        {
            _fish = fish;
            _fishImage.sprite = fish.Sprite;
            UpdateItem();
        }

        private void UpdateItem()
        {
            _fishNameText.text = _fish.Name;
            _fishCountText.text = _fish.Count.ToString();
        }
    }
}