using System;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class TimerEffectData
    {
        public float timer;
        public float tickRate;
        public ITimerEffect timerEffect;

        public Entity source;

        public TimerEffectData(ITimerEffect timerEffect, Entity source)
        {
            this.timerEffect = timerEffect;
            this.source = source;
            timer = 0;
        }

        public void OnTick()
        {
            timer += Time.deltaTime;
            if (timer >= tickRate)
            {
                var targets = timerEffect.GetTargets();
                timerEffect.OnTick(source, targets);
                timer = 0;
            }
        }
    }
}
