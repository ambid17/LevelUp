using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSpeedEffect", menuName = "ScriptableObjects/Effects/ProjectileSpeedEffect", order = 1)]
    [Serializable]
    public class ProjectileSpeedEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.projectileMoveSpeed;
        }
    }
}