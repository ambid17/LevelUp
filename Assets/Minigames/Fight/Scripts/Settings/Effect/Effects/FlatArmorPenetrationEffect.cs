using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "FlatArmorPenetrationEffect", menuName = "ScriptableObjects/Fight/Effects/FlatArmorPenetrationEffect", order = 1)]
    [Serializable]
    public class FlatArmorPenetrationEffect : Effect
    {
        public float flatPenPerStack;
        private float Total => flatPenPerStack * AmountOwned;
        
        private readonly string _description = "Grants +{0} armor penetration";
        public override string Description => string.Format(_description, flatPenPerStack);

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;

        public override void Execute(HitData hit)
        {
            hit.FlatArmorPenetration += Total;
        }
        
        public override void Unlock(EffectSettings settings)
        {
            
        }
    }
}
