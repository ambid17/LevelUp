using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseSpeedEffect", menuName = "ScriptableObjects/Fight/Effects/BaseSpeedEffect", order = 1)]
    [Serializable]
    public class BaseSpeedEffect : StatModifierEffect
    {
        public override StatImpactType statImpactType => StatImpactType.Flat;

        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.movementStats.moveSpeed;
        }
    }
}