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
        
        public List<float> BaseDamageAdditions;
        public List<float> BaseDamageMultipliers;
        public float FlatArmorPenetration;
        public float FlatMagicPenetration;
        public List<float> EffectDamages; 

        public HitData(Entity source, float damage)
        {
            Source = source;

            BaseDamage = damage;
            Effects = Source.Stats.OnHitEffects;

            BaseDamageAdditions = new();
            BaseDamageMultipliers = new();
            EffectDamages = new();
        }

        // Base damage * [weaponMult] + [effectDamage * effectMult]... - (armor * penetration)
        public float CalculateDamage(Entity target)
        {
            Target = target;
            foreach (var effect in Effects)
            {
                // populates the list of damages/multipliers
                effect.Execute(this);
            }

            float physicalDamage = BaseDamage;

            // ex: +5 damage
            foreach (var baseDmgAddtion in BaseDamageAdditions)
            {
                physicalDamage += baseDmgAddtion;
            }
            
            // ex: +10% physical damage, +50% physical damage if enemy <50% hp
            foreach (var dmgMultiplier in BaseDamageMultipliers)
            {
                physicalDamage *= dmgMultiplier;
            }

            physicalDamage -= (Target.Stats.armor - FlatArmorPenetration);

            float magicDamage = 0;
            // ex: +10 lightning damage on hit
            foreach (var effectDmg in EffectDamages)
            {
                magicDamage += effectDmg;
            }

            magicDamage -= (Target.Stats.magicResistance - FlatMagicPenetration);

            float totalDamage = physicalDamage + magicDamage;
            
            return totalDamage;
        }
    }
}