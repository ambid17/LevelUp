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
        private readonly string _description = "Deal {0}% more damage to enemies <{1}% hp";
        public override string Description => string.Format(_description, percentDamagePerStack * 100, executePercent * 100);
        public override string NextUpgradeDescription => "";
        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/effect/ice/execute";

        public override void Execute(HitData hit)
        {
            if (hit.Target.Stats.currentHp / hit.Target.Stats.maxHp < executePercent)
            {
                hit.BaseDamageMultipliers.Add(Total);
            }
        }
    }
}