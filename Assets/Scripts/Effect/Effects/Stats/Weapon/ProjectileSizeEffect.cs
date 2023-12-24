using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSizeEffect", menuName = "ScriptableObjects/Effects/ProjectileSizeEffect", order = 1)]
    [Serializable]
    public class ProjectileSizeEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.projectileSize;
        }
    }
}