using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseDamageEffect", menuName = "ScriptableObjects/Fight/Effects/BaseDamageEffect", order = 1)]
    [Serializable]
    public class BaseDamageEffect : Effect
    {
        public float baseDamagePerStack;

        private float Total => baseDamagePerStack * AmountOwned;

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public int Order => ExecutionOrder;

        public override void Execute(HitData hit)
        {
            hit.BaseDamageAdditions.Add(Total);
        }
    }
}