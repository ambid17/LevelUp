using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HealOverTimeEffect : Effect, ITimerEffect
    {
        public float HealAmount;
        public float TickRate { get; }

        public override void OnCraft(Entity target)
        {
            target.Stats.combatStats.AddTimerEffect(this, target);
        }

        public void OnTick(Entity source, List<Entity> targets)
        {
            source.Stats.combatStats.AddHp(HealAmount);
        }

        // TODO: can use Physics.overlapCircle for now...
        // but we will want to Generate a list of all active entities
        // and just filtering that list down
        public List<Entity> GetTargets()
        {
            return null;
        }
    }
}
