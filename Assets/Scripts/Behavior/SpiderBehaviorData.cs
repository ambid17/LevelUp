using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class SpiderBehaviorData : EntityBehaviorData
    {
        [SerializeField]
        private float playerSlowedMultiplier = 2;
        [SerializeField]
        private float optimalDistance = 15;
        [SerializeField]
        private float baseAttackPriorityFactor;

        public bool IsTargetSlowed
        {
            get
            {
                foreach (var effect in GameManager.PlayerEntity.Stats.movementStats.moveSpeed.statusEffects)
                {
                    if (effect.statusEffect is SlowEffect)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public float DistanceComparedToOptimal => optimalDistance - DistanceToPlayer;
        public float AttackPriority => optimalDistance * baseAttackPriorityFactor * (IsTargetSlowed ? playerSlowedMultiplier : 1);
        public float RetreatPriority => DistanceComparedToOptimal + DamageLastTick;
    }
}