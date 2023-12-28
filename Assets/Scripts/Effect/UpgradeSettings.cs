using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "UpgradeSettings", menuName = "ScriptableObjects/UpgradeSettings", order = 1)]
    [Serializable]
    public class UpgradeSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<Upgrade> AllUpgrades;

        public void SetDefaults()
        {
            foreach (var upgrade in AllUpgrades)
            {
                upgrade.SetDefaults();
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
            var upgradeToLoad = AllUpgrades.FirstOrDefault(upgrade => upgrade.Name == upgradeModel.Name);

            if (upgradeToLoad != null)
            {
                upgradeToLoad.IsUnlocked = upgradeModel.IsUnlocked;
                upgradeToLoad.AmountOwned = upgradeModel.AmountOwned;
                upgradeToLoad.IsCrafted = upgradeModel.IsCrafted;
            }
            else
            {
                Debug.LogError($"No upgrade found with name: {upgradeModel.Name}");
            }
        }

        public void CraftSavedEffects() 
        {
            foreach(var upgrade in AllUpgrades)
            {
                upgrade.Init();
                if(upgrade.IsCrafted)
                {
                    upgrade.Craft();
                }
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

        [ContextMenu("Populate Upgrades")]
        public void FindAndPopulateUpgrades()
        {
            AllUpgrades = new();

            var upgradeGuids = AssetDatabase.FindAssets("t:scriptableobject", new[] { "Assets/ScriptableObjects/Upgrades" });

            foreach (var guid in upgradeGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var upgrade = AssetDatabase.LoadAssetAtPath<Upgrade>(assetPath);
                AllUpgrades.Add(upgrade);
            }

            EditorUtility.SetDirty(this);
        }


        [ContextMenu("Mark upgrades and effects dirty")]
        public void MarkDirty()
        {
            foreach(var upgrade in AllUpgrades)
            {
                EditorUtility.SetDirty(upgrade);

                if(upgrade.positive.effect != null)
                {
                    EditorUtility.SetDirty(upgrade.positive.effect);
                }

                if (upgrade.negative.effect != null)
                {
                    EditorUtility.SetDirty(upgrade.negative.effect);
                }
            }
        }
    }
}