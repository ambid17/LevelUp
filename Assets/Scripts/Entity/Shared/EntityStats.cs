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
        public float DamageTakenThisSecond;
        public List<StatusEffectInstance> StatusEffects = new();
        public List<Effect> OnHitEffects = new();

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            DamageTakenThisSecond += damage;
        }

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
            OnHitEffects = enemyStats.effects;
        }
    }
}