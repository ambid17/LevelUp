using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "AmmoRegenRateEffect", menuName = "ScriptableObjects/Fight/Effects/AmmoRegenRateEffect", order = 1)]
    [Serializable]
    public class AmmoRegenRateEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.projectileWeaponStats.ammoRegenRate;
        }
    }
}