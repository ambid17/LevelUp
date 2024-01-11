using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class UpgradeContainer
    {
        public List<UpgradeModel> upgrades;

        public UpgradeContainer()
        {
        }

        public UpgradeContainer(List<Upgrade> upgrades)
        {
            foreach (var upgrade in upgrades)
            {
                TrackUpgrade(upgrade);
            }
        }

        private void TrackUpgrade(Upgrade upgrade)
        {
            if (upgrades == null)
            {
                upgrades = new List<UpgradeModel>();
            }

            UpgradeModel newEffect = new UpgradeModel()
            {
                Type = upgrade.GetType(),
                Name = upgrade.Name,
                AmountOwned = upgrade.AmountOwned,
                IsUnlocked = upgrade.IsUnlocked
            };

            upgrades.Add(newEffect);
        }
    }
}
