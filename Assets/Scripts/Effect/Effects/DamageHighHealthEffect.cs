using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// Ex: +10% damage to enemies >60% hp
    /// </summary>
    [CreateAssetMenu(fileName = "DamageHighHealthEffect", menuName = "ScriptableObjects/Fight/Effects/DamageHighHealthEffect", order = 1)]
    [Serializable]
    public class DamageHighHealthEffect : StatModifierEffect
    {
        public float minHpPercent;

        public override StatImpactType statImpactType => StatImpactType.Compounding;

        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            //     return entity.Stats.combatStats.baseDamage;
            return null;
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnHitEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnHitEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            if (target.Stats.combatStats.currentHp / target.Stats.combatStats.maxHp.Calculated > minHpPercent)
            {
               // source.Stats.combatStats.onHitDamage.AddSingleUseEffect(this);
            }
        }
    }
}