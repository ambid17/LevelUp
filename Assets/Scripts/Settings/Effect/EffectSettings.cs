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

        public List<Effect> OnHitEffects = new();

        public void SetDefaults()
        {
            foreach (var upgrade in AllUpgrades)
            {
                upgrade.AmountOwned = 0;
                upgrade.IsUnlocked = false;
            }
        }

        public void UnlockAllUpgrades()
        {
            foreach (var upgrade in AllUpgrades)
            {
                upgrade.Unlock();
            }
        }

        public void LoadSavedUpgrade(UpgradeModel upgradeModel)
        {
            Type upgradeType = upgradeModel.Type;
            var upgradeToLoad = AllUpgrades.FirstOrDefault(e => e.GetType() == upgradeType);

            if (upgradeToLoad != null)
            {
                upgradeToLoad.IsUnlocked = true;
                upgradeToLoad.Craft(upgradeModel.AmountOwned);
            }
            else
            {
                Debug.LogError($"No upgrade found of type: {upgradeType}");
            }
        }


        [NonSerialized] private int _weightTotal;

        public Upgrade GetRandomUpgrade()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = AllUpgrades.Sum(e => e.DropWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var upgrade in AllUpgrades)
            {
                randomWeight -= upgrade.DropWeight;
                if (randomWeight < 0)
                {
                    return upgrade;
                }
            }

            return AllUpgrades[0];
        }

        public List<Upgrade> GetRandomUpgrades(int count)
        {
            List<Upgrade> toReturn = new();

            while (toReturn.Count < count)
            {
                Upgrade random = GetRandomUpgrade();
                if (!toReturn.Contains(random))
                {
                    toReturn.Add(random);
                }
            }

            return toReturn;
        }
    }
}