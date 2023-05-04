using System;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "DamageOverTimeEffect", menuName = "ScriptableObjects/Fight/DamageOverTimeEffect", order = 1)]
    [Serializable]
    public class DamageOverTimeEffect : Effect, IStatusEffect
    {
        public float chance = 0.1f;
        public float duration = 2f;
        public float Duration => duration;
        public float baseDamage = 5f;
        public float damageScalar = 1f;
        public float tickRate = 1f;
        public float TickRate => tickRate;

        public float HitDamage => baseDamage + (damageScalar * AmountOwned);
        
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
            int newAmountOwned = AmountOwned + purchaseCount;
            return baseDamage + (damageScalar * newAmountOwned);
        }

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/effect/fire/burn";

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            bool doesApply = Random.value < chance;
            if (doesApply)
            {
                StatusEffectInstance.Create(hit, this);
            }
        }

        public void ApplyEffect(Entity target)
        {
        }

        public void RemoveEffect(Entity target)
        {
        }

        public void OnTick(Entity target)
        {
            target.TakeDamage(HitDamage);
        }
    }
}