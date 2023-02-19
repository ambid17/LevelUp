using System;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Fight/SlowEffect", order = 1)]
    [Serializable]
    public class SlowEffect : Effect, IStatusEffect, IExecuteEffect
    {
        public float slowChance = 1f;
        public float duration = 2f;
        public float slowAmount = 0.01f;

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;

        public void Execute(HitData hit)
        {
            TryAdd(hit);
        }

        public void TryAdd(HitData hit)
        {
            bool doesSlow = Random.value < slowChance;
            doesSlow = true;
            if (doesSlow)
            {
                StatusEffectInstance.Create(hit.Source, hit.Target, this, duration);
            }
        }

        public void OnAdd(Entity target)
        {
            target.MovementController.ApplyMoveEffect(slowAmount);
        }

        public void OnRemove(Entity target)
        {
            target.MovementController.RemoveMoveEffect(slowAmount);
        }

        public void OnTick()
        {
            
        }
    }
}