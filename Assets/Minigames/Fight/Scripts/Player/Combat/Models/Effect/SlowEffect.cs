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

        public void Execute(DamageWorksheet worksheet)
        {
            TryAdd(worksheet);
        }

        public void TryAdd(DamageWorksheet worksheet)
        {
            bool doesSlow = Random.value < slowChance;
            doesSlow = true;
            if (doesSlow)
            {
                StatusEffectTracker.AddTracker(worksheet.source, worksheet.target, this, duration);
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
    }
}