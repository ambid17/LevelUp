using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSpreadEffect", menuName = "ScriptableObjects/Fight/Effects/ProjectileSpreadEffect", order = 1)]
    [Serializable]
    public class ProjectileSpreadEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.projectileSpread;
        }
    }
}