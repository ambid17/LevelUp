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
        
        private readonly string _description = "{0}% to slow enemies by {1}% for {2} seconds";
        public override string Description => string.Format(_description, slowChance * 100, slowAmount * 100, duration);

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/effect/ice/slow";

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            bool doesSlow = Random.value < SlowChance;
            doesSlow = true;
            if (doesSlow)
            {
                StatusEffectInstance.Create(hit.Source, hit.Target, this, duration);
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

        public void OnTick()
        {
            
        }
        
        public override void Unlock(EffectSettings settings)
        {
            if (!settings.UnlockedEffects.Contains(this))
            {
                AmountOwned = 1;
                settings.UnlockedEffects.Add(this);
                settings.OnHitEffects.Add(this);
            }
            else
            {
                AmountOwned++;
            }
        }
    }
}