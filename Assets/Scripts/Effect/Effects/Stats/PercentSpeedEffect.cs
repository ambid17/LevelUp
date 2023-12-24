using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentSpeedEffect : StatModifierEffect
{
    public override StatImpactType statImpactType => StatImpactType.Compounding;

    public override ModifiableStat GetStatToAffect(Entity entity)
    {
        return entity.Stats.movementStats.moveSpeed;
    }
}
