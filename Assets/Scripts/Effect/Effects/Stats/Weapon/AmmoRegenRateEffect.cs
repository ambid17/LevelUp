using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "AmmoRegenRateEffect", menuName = "ScriptableObjects/Effects/Weapon/AmmoRegenRateEffect", order = 1)]
    [Serializable]
    public class AmmoRegenRateEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.ammoRegenRate;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.ammoRegenRate;
            }
        }
    }
}