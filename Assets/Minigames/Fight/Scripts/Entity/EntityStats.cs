using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public float currentHp;
        public OrderedList<StatusEffectInstance> StatusEffects = new();
        public OrderedList<IExecuteEffect> OnHitEffects = new();
    }
}