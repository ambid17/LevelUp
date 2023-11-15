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

        public bool IsTargetSlowed
        {
            get
            {
                foreach (StatusEffectInstance effectInstance in GameManager.PlayerEntity.Stats.StatusEffects)
                {
                    if (effectInstance.effect is SlowEffect)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public float DistanceComparedToOptimal => optimalDistance - DistanceToPlayer;
        public float AttackPriority => -DistanceComparedToOptimal * (IsTargetSlowed ? playerSlowedMultiplier : 1);
        public float RetreatPriority => DistanceComparedToOptimal + DamageLastTick;
    }
}