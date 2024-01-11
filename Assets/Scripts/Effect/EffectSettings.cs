using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "EffectSettings", menuName = "ScriptableObjects/Fight/EffectSettings", order = 1)]
    [Serializable]
    public class EffectSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<Upgrade> AllUpgrades;

        public void SetDefaults()
        {
            foreach (var upgrade in AllUpgrades)
            {
                upgrade.AmountOwned = 0;
                upgrade.IsUnlocked = false;
                upgrade.IsCrafted = false;
            }
        }

        public void UnlockAllUpgrades()
        {
            foreach (var upgrade in AllUpgrades)
            {
                upgrade.Unlock();
            }
        }

        public List<Upgrade> GetAllUpgradesInCategory(UpgradeCategory upgradeCategory, EffectCategory effectCategory, TierCategory tierCategory)
        {
            return AllUpgrades.Where(e =>
                e.UpgradeCategory == upgradeCategory &&
                e.EffectCategory == effectCategory &&
                e.TierCategory == tierCategory
            ).ToList();
        }

        public void LoadSavedUpgrade(UpgradeModel upgradeModel)
        {
            Type upgradeType = upgradeModel.Type;
            var upgradeToLoad = AllUpgrades.FirstOrDefault(e => e.GetType() == upgradeType);

            if (upgradeToLoad != null)
            {
                upgradeToLoad.IsUnlocked = upgradeModel.IsUnlocked;
                upgradeToLoad.AmountOwned = upgradeModel.AmountOwned;
                upgradeToLoad.IsCrafted = upgradeModel.IsCrafted;

                if (upgradeToLoad.IsCrafted)
                {
                    upgradeToLoad.Craft();
                }
            }
            else
            {
                Debug.LogError($"No upgrade found of type: {upgradeType}");
            }
        }

        // Uses a weighted random algorithm to get a locked upgrade, filtered by category
        public Upgrade GetUpgradeToUnlock(UpgradeCategory upgradeCategory, EffectCategory effectCategory, TierCategory tierCategory)
        {
            var lockedUpgrades = AllUpgrades.Where(e =>
                e.UpgradeCategory == upgradeCategory &&
                e.EffectCategory == effectCategory &&
                e.TierCategory == tierCategory &&
                e.IsUnlocked == false
            ).ToList();
            int _weightTotal = lockedUpgrades.Sum(e => 1);

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var upgrade in lockedUpgrades)
            {
                randomWeight -= 1;
                if (randomWeight < 0)
                {
                    return upgrade;
                }
            }

            return lockedUpgrades[0];
        }
    }
}