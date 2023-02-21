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
        public float slowChance = 1f;
        public float duration = 2f;
        public float slowAmount = 0.01f;

        public float SlowChance => slowChance * AmountOwned;

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public int Order => ExecutionOrder;

        public override void Execute(HitData hit)
        {
            TryAdd(hit);
        }

        public void TryAdd(HitData hit)
        {
            bool doesSlow = Random.value < SlowChance;
            doesSlow = true;
            if (doesSlow)
            {
                StatusEffectInstance.Create(hit.Source, hit.Target, this, duration);
            }
        }

        public void OnAdd(Entity target)
        {
            target.MovementController.ApplyMoveEffect(this);
        }

        public void OnRemove(Entity target)
        {
            target.MovementController.RemoveMoveEffect(this);
        }

        public void OnTick()
        {
            
        }
    }
}