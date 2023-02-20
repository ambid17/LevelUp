using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public float currentHp;
        public float armor;
        public float magicResistance;
        public OrderedList<StatusEffectInstance> StatusEffects = new();
        public List<StatusEffectInstance> StatusEffectsToRemove = new();

        public bool AddStatusEffect(StatusEffectInstance instance)
        {
            if (StatusEffects.Contains(instance))
            {
                StatusEffects[StatusEffects.IndexOf(instance)].remainingTime = instance.remainingTime;
                return false;
            }
            else
            {
                StatusEffects.Add(instance);
                return true;
            }
        }
        
        public OrderedList<IExecuteEffect> OnHitEffects = new();
    }
}