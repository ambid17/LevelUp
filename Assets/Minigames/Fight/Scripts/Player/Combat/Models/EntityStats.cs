using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public float currentHp;
        public float maxHp;

        public float armor;
        public float magicResist;

        

        public OrderedList<StatusEffectTracker> StatusEffects = new();
    }
}