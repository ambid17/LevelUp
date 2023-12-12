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
        [Header("Effect specific")]
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

        public override void OnCraft(Entity target)
        {
            target.Stats.combatStats.OnHitEffects.Add(this);
        }

        public override void Execute(Entity source, Entity target)
        {
            bool doesStun = Random.value < StunChance;
            if (doesStun)
            {
                target.Stats.movementStats.moveSpeed.AddOrRefreshStatusEffect(this, source, target);
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * 0;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
        }
    }
}