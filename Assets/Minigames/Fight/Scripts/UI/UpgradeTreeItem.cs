using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class UpgradeTreeItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        public Upgrade upgrade;
    
        private EventService _eventService;
        
        private void Start()
        {
            _eventService = GameManager.EventService;
        }

        public void Setup(Upgrade upgrade)
        {
            this.upgrade = upgrade;
            icon.sprite = upgrade.icon;
            button.onClick.AddListener(InspectUpgrade);
        }

        private void InspectUpgrade()
        {
            _eventService.Dispatch(new UpgradeSelectedEvent(upgrade));
        }
    }
}
