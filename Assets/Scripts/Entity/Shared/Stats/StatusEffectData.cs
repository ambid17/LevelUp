using System;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class StatusEffectData
    {
        public float timer;
        public float tickTimer;
        public IStatusEffect statusEffect;
        public Entity source;
        public Entity target;

        public StatusEffectData(IStatusEffect statusEffect, Entity source, Entity target)
        {
            this.statusEffect = statusEffect;
            this.source = source;
            this.target = target;

            timer = 0;
        }

        public bool OnTick()
        {
            timer += Time.deltaTime;
            tickTimer += Time.deltaTime;

            if (tickTimer > statusEffect.TickRate)
            {
                statusEffect.OnTick(source, target);
                tickTimer = 0;
            }

            bool didFinish = timer >= statusEffect.Duration;
            if (didFinish)
            {
                statusEffect.OnComplete();
            }

            return didFinish;
        }

        public void Reapply()
        {
            timer = 0;
            // TODO figure out how the fuck to diferentiate status effects
            // that do or don't reapply on tick.
        }
    }
}
