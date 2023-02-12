using System;
using Minigames.Fight;

public enum DamageType
{
    Physical, Magic
}

public class DamageEffect : Effect, IExecuteEffect
{
    public DamageType DamageType;
    
    public override EffectTriggerType TriggerType { get; }
    public void Execute(DamageWorksheet worksheet)
    {
        throw new NotImplementedException();
    }
}