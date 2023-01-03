using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Mining
{
    public class OreMarketUI : MonoBehaviour
    {
        [SerializeField] OreMarketItem _itemPrefab;
        [SerializeField] Transform _parent;

        private void Start()
        {
            SetupItemGroups();
        }

        private void SetupItemGroups()
        {
            foreach(var ore in GameManager.TileSettings.Tiles)
            {
                OreMarketItem item = Instantiate(_itemPrefab, _parent);
                item.Setup(ore);
            }
        }
    }

}
