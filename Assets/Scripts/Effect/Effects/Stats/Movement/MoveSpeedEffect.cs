using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "MoveSpeedEffect", menuName = "ScriptableObjects/Effects/MoveSpeedEffect", order = 1)]
    [Serializable]
    public class MoveSpeedEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.movementStats.moveSpeed;
        }
    }
}