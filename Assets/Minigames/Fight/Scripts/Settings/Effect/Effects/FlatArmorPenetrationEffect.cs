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

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public int Order => ExecutionOrder;

        public override void Execute(HitData hit)
        {
            hit.FlatArmorPenetration += Total;
        }
    }
}
