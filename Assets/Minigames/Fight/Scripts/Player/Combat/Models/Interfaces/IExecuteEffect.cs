namespace Minigames.Fight
{
    public interface IExecuteEffect
    {
        public EffectTriggerType TriggerType { get; }
        public void Execute(DamageWorksheet worksheet);
    }
}