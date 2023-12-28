using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "MaxAmmoEffect", menuName = "ScriptableObjects/Effects/Weapon/MaxAmmoEffect", order = 1)]
    [Serializable]
    public class MaxAmmoEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.maxAmmo;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.maxAmmo;
            }
        }

        public override string GetStatName()
        {
            return "Max Ammo";
        }
    }
}