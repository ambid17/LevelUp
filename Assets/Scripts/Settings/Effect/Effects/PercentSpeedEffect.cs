using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentSpeedEffect : Effect
{
    [Header("Effect specific")]
    public float percentPerStack;

    private float Impact => 1 + (percentPerStack * AmountOwned);

    private readonly string _description = "Impacts player move speed by {0}%";

    public override string GetDescription()
    {
        return string.Format(_description, Impact * 100);
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

    public override void OnCraft(Entity target)
    {
        target.Stats.movementStats.moveSpeed.AddEffect(this);
    }

    public override float ImpactStat(float stat)
    {
        return stat * Impact;
    }
}
