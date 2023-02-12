using System;
using Minigames.Fight;

public class DamageEffect : Effect, IExecuteEffect
{
    public DamageType DamageType;
    
    public override EffectTriggerType TriggerType { get; }
    public void Execute(DamageWorksheet worksheet)
    {
        throw new NotImplementedException();
    }
}