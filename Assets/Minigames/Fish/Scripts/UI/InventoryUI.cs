using System.Collections;
using System.Collections.Generic;
using Minigames.Fish;
using UnityEngine;

namespace Minigames.Fish
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private MarketItem marketItemPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _container;

        void Start()
        {
            TogglePanel(false);
        }



        public void TogglePanel(bool shouldBeActive)
        {
            _container.SetActive(shouldBeActive);
        }
    }
}