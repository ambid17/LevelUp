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

        private readonly string _description = "+{0} base damage";
        public override string Description => string.Format(_description, baseDamagePerStack);
        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "";
        public override void Execute(HitData hit)
        {
            hit.BaseDamageAdditions.Add(Total);
        }
        
        public override void Unlock(EffectSettings settings)
        {
            
        }
    }
}