using System;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "StunEffect", menuName = "ScriptableObjects/Fight/StunEffect", order = 1)]
    [Serializable]
    public class StunEffect : Effect, IStatusEffect
    {
        public float stunChance = 0.1f;
        public float duration = 2f;
        public float Duration => duration;
        public float chanceScalar = 0.01f;
        public float TickRate => 0;

        public float StunChance => stunChance + (chanceScalar * AmountOwned);
        
        private readonly string _description = "{0}% to stun enemies for {1} seconds";

        public override string GetDescription()
        {
            return string.Format(_description, StunChance * 100, duration);
        }
        
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100, duration);
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return stunChance + (chanceScalar * newAmountOwned);
        }

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/effect/ice/stun";

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            bool doesSlow = Random.value < StunChance;
            doesSlow = true;
            if (doesSlow)
            {
                StatusEffectInstance.Create(hit.Source, hit.Target, this);
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