using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "FlatMagicPenetrationEffect", menuName = "ScriptableObjects/Fight/Effects/FlatMagicPenetrationEffect", order = 1)]
    [Serializable]
    public class FlatMagicPenetrationEffect : Effect
    {
        public float flatPenPerStack;
        private float Total => flatPenPerStack * AmountOwned;
        
        private readonly string _description = "Grants +{0} magic penetration";
        public override string Description => string.Format(_description, flatPenPerStack);

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "";

        public override void Execute(HitData hit)
        {
            hit.FlatMagicPenetration += Total;
        }
        
        public override void Unlock(EffectSettings settings)
        {
            
        }
    }
}
