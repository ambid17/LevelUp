using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ExplosionEffect", menuName = "ScriptableObjects/Effects/ExplosionEffect", order = 1)]
    [Serializable]
    public class ExplosionEffect : AoeEffect
    {

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            base.ApplyOverrides(overrides);
        }

        public override void TryPlace(ProjectileController projectile)
        {
            base.TryPlace(projectile);
            if (projectileAoeMappings[projectile])
            {
                bool shouldBackfire = Random.value < chanceToBackfire;
                if (shouldBackfire) 
                {
                    projectile.MyWeaponStats.projectileLifeTime.AddSingleUseEffect(negative as StatModifierEffect);
                }
            }
        }
    }
}