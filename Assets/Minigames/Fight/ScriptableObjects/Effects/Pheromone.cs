using System;
using UnityEngine;
namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Fight/PheromoneEffect", order = 1)]
    [Serializable]
    public class PheromoneEffect : Effect, IStatusEffect
    {
        public float duration = 2f;
        public float Duration => duration;
        public float TickRate => 0;

        private readonly string _description = "Marked with pheromones for {0} seconds";


        public override string GetDescription()
        {
            return string.Format(_description, duration);
        }

        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Empty;
        }

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => string.Empty;

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            StatusEffectInstance.Create(hit.Source, hit.Target, this);
        }

        public void ApplyEffect(Entity target)
        {
            
        }

        public void RemoveEffect(Entity target)
        {
            
        }

        public void OnTick(Entity target)
        {

        }
    }
}