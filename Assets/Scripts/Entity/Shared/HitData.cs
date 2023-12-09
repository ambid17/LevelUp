using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class HitData
    {
        public Entity Source;
        public Entity Target;

        public float BaseDamage;
        public List<Effect> Effects;

        public float BaseDamageAddition;
        public float DamageMultiplier;
        public List<float> EffectDamages;

        public HitData(Entity source, float damage)
        {
            Source = source;

            BaseDamage = damage;
            Effects = Source.Stats.OnHitEffects;

            BaseDamageAddition = 0;
            DamageMultiplier = 1;
            EffectDamages = new();
        }

        public float CalculateDamage(Entity target)
        {
            Target = target;
            foreach (var effect in Effects)
            {
                // populates the lists of damages/multipliers
                effect.Execute(this);
            }

            float totalDamage = (BaseDamage + BaseDamageAddition) * DamageMultiplier;

            return totalDamage;
        }
    }
}