using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "StatusEffectApplier", menuName = "ScriptableObjects/Fight/StatusEffectApplier",
        order = 1)]
    [Serializable]
    public class StatusEffectApplier : Effect, IExecuteEffect
    {
        [Tooltip("Must have IStatusEffect")] public List<Effect> statusEffects;

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;

        public void Execute(DamageWorksheet worksheet)
        {
            foreach (IStatusEffect effect in statusEffects)
            {
                effect.TryAdd(worksheet);
            }
        }
    }
}