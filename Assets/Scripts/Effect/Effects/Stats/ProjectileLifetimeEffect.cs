using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileLifetimeEffect", menuName = "ScriptableObjects/Fight/Effects/ProjectileLifetimeEffect", order = 1)]
    [Serializable]
    public class ProjectileLifetimeEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.projectileLifeTime;
        }
    }
}