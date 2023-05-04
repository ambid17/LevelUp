using System;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Fight/SlowEffect", order = 1)]
    [Serializable]
    public class SlowEffect : Effect, IStatusEffect
    {
        public float slowChance = 0.1f;
        public float duration = 2f;
        public float Duration => duration;
        public float slowAmount = 0.01f;
        public float chanceScalar = 0.01f;
        public float TickRate => 0;

        public float SlowChance => slowChance + (chanceScalar * AmountOwned);
        
        private readonly string _description = "{0}% to slow enemies by {1}% for {2} seconds";
        

        public override string GetDescription()
        {
            return string.Format(_description, SlowChance * 100, slowAmount * 100, duration);
        } 

        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100, slowAmount * 100, duration);
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return slowChance + (chanceScalar * newAmountOwned);
        }

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/effect/ice/slow";

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            bool doesSlow = Random.value < SlowChance;
            if (doesSlow)
            {
                StatusEffectInstance.Create(hit, this);
            }
        }

        public void ApplyEffect(Entity target)
        {
            target.MovementController.ApplyMoveEffect(this);
        }

        public void RemoveEffect(Entity target)
        {
            target.MovementController.RemoveMoveEffect(this);
        }

        public void OnTick(Entity target)
        {
            
        }
    }
}