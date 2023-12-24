using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "MagSizeEffect", menuName = "ScriptableObjects/Effects/MagSizeEffect", order = 1)]
    [Serializable]
    public class MagSizeEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.maxAmmo;
        }
    }
}