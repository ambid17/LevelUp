using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public abstract class StatusEffect : Effect
    {
        public abstract void TryAdd(HitData hit);
        public abstract void OnRemove(Entity target);
        public abstract void OnAdd(Entity target);
        public abstract void OnTick();
    }
}