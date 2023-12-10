using System;
using Minigames.Fight;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Fight/SlowEffect", order = 1)]
    [Serializable]
    public class SlowEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float duration = 2f;
        public float Duration => duration;
        public float TickRate => 0;

        public float slowAmount = 0.5f;
        public float chanceScalar = 0.01f;
        

        public float slowChance = 0.5f;
        public float SlowChance => slowChance + (chanceScalar * AmountOwned);
        
        private readonly string _description = "{0}% chance to slow enemies by {1}% for {2} seconds";
        
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

        public override void OnCraft(Entity target)
        {
            target.Stats.combatStats.OnHitEffects.Add(this);
        }

        public override void Execute(Entity source, Entity target)
        {
            bool doesSlow = Random.value < SlowChance;
            if (doesSlow)
            {
                target.Stats.movementStats.moveSpeed.AddOrRefreshStatusEffect(this, source, target);
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * slowAmount;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
        }
    }
}