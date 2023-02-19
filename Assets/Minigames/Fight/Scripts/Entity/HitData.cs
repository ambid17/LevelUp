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
        public List<IExecuteEffect> Effects;
        
        public List<float> BaseDamageAdditions;
        public List<float> BaseDamageMultipliers;
        public List<float> EffectDamages; 

        public HitData(Entity source, Entity target)
        {
            Source = source;
            Target = target;

            BaseDamageAdditions = new();
            BaseDamageMultipliers = new();
            EffectDamages = new();
        }

        // Base damage * [weaponMult] + [effectDamage * effectMult]... - (armor * penetration)
        public float CalculateDamage()
        {
            foreach (var effect in Effects)
            {
                // populates the list of damages/multipliers
                effect.Execute(this);
            }

            float totalDamage = BaseDamage;

            // ex: +5 damage
            foreach (var baseDmgAddtion in BaseDamageAdditions)
            {
                totalDamage += baseDmgAddtion;
            }
            
            // ex: +10% damage, +50% damage if enemy <50% hp
            foreach (var dmgMultiplier in BaseDamageMultipliers)
            {
                totalDamage *= dmgMultiplier;
            }
            
            // ex: +10 lightning damage on hit
            foreach (var effectDmg in EffectDamages)
            {
                totalDamage += effectDmg;
            }

            return totalDamage;
        }
    }
}