using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ExecuteEffect", menuName = "ScriptableObjects/Fight/Effects/ExecuteEffect", order = 1)]
    [Serializable]
    public class ExecuteDamageEffect : Effect
    {
        public float percentDamagePerStack;
        public float executePercent;

        private float Total => 1 + (percentDamagePerStack * AmountOwned);

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public int Order => ExecutionOrder;

        public override void Execute(HitData hit)
        {
            if (hit.Target.Stats.currentHp / hit.Target.Stats.maxHp < executePercent)
            {
                hit.BaseDamageMultipliers.Add(Total);
            }
        }
    }
}