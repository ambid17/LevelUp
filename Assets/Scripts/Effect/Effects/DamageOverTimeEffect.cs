using System;
using Minigames.Fight;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "DamageOverTimeEffect", menuName = "ScriptableObjects/Fight/DamageOverTimeEffect", order = 1)]
    [Serializable]
    public class DamageOverTimeEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float chance = 0.1f;
        public float duration = 2f;
        public float Duration => duration;
        public float baseDamage = 5f;
        public float damageScalar = 1f;
        public float tickRate = 1f;
        public float TickRate => tickRate;

        public float HitDamage => baseDamage + (damageScalar * _amountOwned);
        
        private readonly string _description = "{0}% to burn enemies for {1} damage each second for {2} seconds";
        
        public override string GetDescription()
        {
            return string.Format(_description, chance * 100, HitDamage, duration);
        } 

        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, chance * 100, NextUpgrade(purchaseCount), duration);
        } 

        private float NextUpgrade(int purchaseCount)
        {
            int newAmountOwned = _amountOwned + purchaseCount;
            return baseDamage + (damageScalar * newAmountOwned);
        }

        public override void OnCraft(Entity target)
        {
            if(_upgradeCategory == UpgradeCategory.Range)
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
            bool doesApply = Random.value < chance;
            if (doesApply)
            {
                target.Stats.combatStats.AddOrRefreshStatusEffect(this, source, target);
            }
        }

        public void OnTick(Entity source, Entity target)
        {
            target.TakeHit(HitDamage, source);
        }

        public void OnComplete()
        {
        }
    }
}