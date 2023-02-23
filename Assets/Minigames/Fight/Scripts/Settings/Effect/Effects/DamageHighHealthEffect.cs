using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// Ex: +10% damage to enemies >60% hp
    /// </summary>
    [CreateAssetMenu(fileName = "DamageHighHealthEffect", menuName = "ScriptableObjects/Fight/Effects/DamageHighHealthEffect", order = 1)]
    [Serializable]
    public class DamageHighHealthEffect : Effect
    {
        public float percentDamagePerStack;
        public float minHpPercent;

        private float Total => 1 + (percentDamagePerStack * AmountOwned);
        
        private readonly string _description = "Deal {0}% more damage to enemies >{1}% hp";
        public override string Description => string.Format(_description, percentDamagePerStack * 100, minHpPercent * 100);

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;

        public override void Execute(HitData hit)
        {
            if (hit.Target.Stats.currentHp / hit.Target.Stats.maxHp > minHpPercent)
            {
                hit.BaseDamageMultipliers.Add(Total);
            }
        }
        
        public override void Unlock()
        {
            
        }
    }
}