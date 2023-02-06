using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class UpgradePurchaseCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Button countButton;

        private EventService _eventService;
        private int purchaseCountIndex;
        private List<int> purchaseCounts = new() { 1, 5, 10, 25, 100, 1000 };

        void Start()
        {
            _eventService = GameManager.EventService;
            countButton.onClick.AddListener(OnButtonClicked);

            purchaseCountIndex = 0;
            int purchaseCount = purchaseCounts[purchaseCountIndex];
            GameManager.SettingsManager.UpgradePurchaseCount = purchaseCount;
            countText.text = $"{purchaseCount}x";
        }

        void OnButtonClicked()
        {
            purchaseCountIndex = ++purchaseCountIndex % purchaseCounts.Count;
            int purchaseCount = purchaseCounts[purchaseCountIndex];
            GameManager.SettingsManager.UpgradePurchaseCount = purchaseCount;
            countText.text = $"{purchaseCount}x";

            _eventService.Dispatch<PurchaseCountChangedEvent>();
        }
    }
}