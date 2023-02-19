using System;
using Minigames.Fight;

namespace Minigames.Fight
{
    public enum DamageType
    {
        Physical,
        Magic
    }

    public class BaseDamageEffect : Effect, IExecuteEffect
    {
        public DamageType DamageType;
        public float baseDamagePerStack;

        private float Total => baseDamagePerStack * AmountOwned;

        public override EffectTriggerType TriggerType { get; }

        public void Execute(HitData hit)
        {
            hit.BaseDamageAdditions.Add(Total);
        }
    }
}