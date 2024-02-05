using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    // AOE Slow
    // AOE fire damage over time
    //  - take damage in AOE, apply status on leaving

    public class AoeEffect : ParentEffect
    {
        public AoeInstanceController aoePrefab;
        public Effect statusEffect;
        public float duration;
        public float tickRate;
        public float size;

        public Dictionary<ProjectileController, bool> projectileAoeMappings = new();

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            base.ApplyOverrides(overrides);
            duration = overrides.duration;
            tickRate = overrides.tickRate;
            size = overrides.maxRange;
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.AoeEffects.Add(this);
            }
            else if( _upgradeCategory == UpgradeCategory.Melee)
            {
                target.Stats.combatStats.meleeWeaponStats.AoeEffects.Add(this);
            }
        }

        public virtual void TryPlace(ProjectileController projectile)
        {
            float value = Random.value;
            bool shouldPlace = value < chanceToApply;

            projectileAoeMappings.Add(projectile, shouldPlace);
        }

        public void Place(Entity source, Vector3 position, ProjectileController projectile)
        { 
            if (projectileAoeMappings[projectile])
            {
                var instance = Instantiate(aoePrefab);
                instance.transform.position = position;
                instance.Setup(source, this);
            }
        }
    }
}