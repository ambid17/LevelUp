using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseDamageEffect", menuName = "ScriptableObjects/Effects/BaseDamageEffect", order = 1)]
    [Serializable]
    public class BaseDamageEffect : StatModifierEffect
    {
        public override StatImpactType statImpactType => StatImpactType.Additive;

        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.baseDamage;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.baseDamage;
            }
        }
    }
}