using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentSpeedIncreaseEffect : Effect
{
    [Header("Effect specific")]
    public float percentPerStack;

    private float Total => 1 + (percentPerStack * AmountOwned);

    private readonly string _description = "Slows player move speed by {0}%";
    public override string GetDescription()
    {
        return string.Format(_description, Total * 100);
    }
    public override string GetNextUpgradeDescription(int purchaseCount)
    {
        return string.Format(_description, NextUpgradeChance(purchaseCount) * 100);
    }

    private float NextUpgradeChance(int purchaseCount)
    {
        int newAmountOwned = AmountOwned + purchaseCount;
        return 1 + (percentPerStack * newAmountOwned);
    }

    public override void Apply(Entity target)
    {
        target.Stats.movementStats.moveSpeed.CompoundingModifiers.Add(Total);
    }

    public override void Execute(Entity target, Entity source)
    {
        
    }
}
