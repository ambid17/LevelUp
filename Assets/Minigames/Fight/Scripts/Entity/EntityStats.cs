using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public float currentHp;
        public float maxHp;
        public float armor;
        public float magicResistance;
        public List<StatusEffectInstance> StatusEffects = new();
        public List<Effect> OnHitEffects = new();

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

        public void SetupFromEnemy(EnemyStats enemyStats)
        {
            currentHp = enemyStats.MaxHp;
            maxHp = enemyStats.MaxHp;
            armor = enemyStats.Armor;
            magicResistance = enemyStats.MagicResist;
        }
    }
}