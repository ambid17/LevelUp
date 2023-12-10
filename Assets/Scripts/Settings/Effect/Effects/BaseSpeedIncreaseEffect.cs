using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpeedIncreaseEffect : Effect
{
    [Header("Effect specific")]
    public float perStack;

    private float Total => (perStack * AmountOwned);

    private readonly string _description = "Increase player move speed by {0}";
    public override string GetDescription()
    {
        return string.Format(_description, Total);
    }
    public override string GetNextUpgradeDescription(int purchaseCount)
    {
        return string.Format(_description, NextUpgradeChance(purchaseCount));
    }

    private float NextUpgradeChance(int purchaseCount)
    {
        int newAmountOwned = AmountOwned + purchaseCount;
        return (perStack * newAmountOwned);
    }

    public override void OnCraft(Entity target)
    {
        target.Stats.movementStats.moveSpeed.BaseModifiers.Add(Total);
    }
}
