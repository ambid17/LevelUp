using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpeedEffect : Effect
{
    [Header("Effect specific")]
    public float perStack;

    private float Impact => perStack * AmountOwned;

    private readonly string _description = "Impacts player move speed by {0}";
    public override string GetDescription()
    {
        return string.Format(_description, Impact);
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
        target.Stats.movementStats.moveSpeed.AddEffect(this);
    }

    public override float ImpactStat(float stat)
    {
        return stat + Impact;
    }
}
