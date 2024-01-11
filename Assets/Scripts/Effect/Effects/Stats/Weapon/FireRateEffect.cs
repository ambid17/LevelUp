using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "FireRateEffect", menuName = "ScriptableObjects/Effects/Weapon/FireRateEffect", order = 1)]
    [Serializable]
    public class FireRateEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.rateOfFire;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.rateOfFire;
            }
        }

        public override string GetStatName()
        {
            return "Fire Rate";
        }
    }
}