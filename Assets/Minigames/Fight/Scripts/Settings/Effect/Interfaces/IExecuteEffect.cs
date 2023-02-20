namespace Minigames.Fight
{
    public interface IExecuteEffect
    {
        public EffectTriggerType TriggerType { get; }
        public int Order { get; }
        public void Execute(HitData hit);
    }
}