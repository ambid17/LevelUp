using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Minigames.Fight {
    public static class EffectUtils
    {
        public static List<Entity> GetEntitiesInRange(Entity target, float range, bool includePlayer, bool includeTarget)
        {
            List<Entity> entitiesInMaxRange = new();

            // Add all of the enemies that can possibly be hit
            var enemiesInMaxRange = GameManager.EnemyObjectPool.ActiveEnemies
                .Select(e => e.MyEntity)
                .Where(e => Vector2.Distance(e.transform.position, target.transform.position) < range);

            // filter down whether or not to include the original target
            if (!includeTarget)
            {
                enemiesInMaxRange = enemiesInMaxRange.Where(e => e != target);
            }

            entitiesInMaxRange.AddRange(enemiesInMaxRange);

            // add the player if it can be hit
            if (includePlayer)
            {
                if (Vector2.Distance(GameManager.PlayerEntity.transform.position, target.transform.position) < range)
                {
                    entitiesInMaxRange.Add(GameManager.PlayerEntity);
                }
            }

            return entitiesInMaxRange;
        }
    }
}