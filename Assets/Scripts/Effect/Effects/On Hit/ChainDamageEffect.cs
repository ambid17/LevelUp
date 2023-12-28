using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ChainDamageEffect", menuName = "ScriptableObjects/Effects/ChainDamageEffect", order = 1)]
    [Serializable]
    public class ChainDamageEffect : Effect
    {
        [Header("Effect specific")]
        public float chanceToApply = 0;
        public float impactPerStack = 1f;
        public float maxRange;
        public float EnemiesImpacted => impactPerStack * _amountOwned;


        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chanceToApply = overrides.applicationChance;
            impactPerStack = overrides.impactPerStack;
            maxRange = overrides.maxRange;
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnHitEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnHitEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            bool doesApply = UnityEngine.Random.value < chanceToApply;
            if (doesApply)
            {
                List<Entity> entitiesInMaxRange = new();

                // Add all of the enemies that can possibly be hit
                var enemiesInMaxRange = GameManager.EnemyObjectPool.ActiveEnemies
                    .Select(e => e.MyEntity)
                    .Where(e => Vector2.Distance(e.transform.position, target.transform.position) < maxRange * EnemiesImpacted && e != target);
                entitiesInMaxRange.AddRange(enemiesInMaxRange);

                // add the player if it can be hit
                if(Vector2.Distance(GameManager.PlayerEntity.transform.position, target.transform.position) < maxRange * EnemiesImpacted)
                {
                    entitiesInMaxRange.Add(GameManager.PlayerEntity);
                }


                // apply damage to the next closest target until we are out of enemies to impact
                var damage = source.Stats.combatStats.projectileWeaponStats.baseDamage.Calculated;
                Entity lastTarget = target;
                for(int i = 0; i < EnemiesImpacted; i++)
                {
                    var newTarget = GetNextTarget(entitiesInMaxRange, lastTarget.transform.position);
                    newTarget.TakeHit(damage, source);

                    // remove from list so we can't target it twice
                    entitiesInMaxRange.Remove(newTarget);
                    lastTarget = newTarget;
                }
            }
        }

        private Entity GetNextTarget(List<Entity> entities, Vector2 targetPosition)
        {
            float minDistance = float.MaxValue;
            Entity closestEntity = null;
            foreach (var entity in entities)
            {
                float distance = Vector2.Distance(entity.transform.position, targetPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEntity = entity;
                }
            }

            return closestEntity;
        }
    }
}