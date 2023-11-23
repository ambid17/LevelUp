namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Sets the SharedAnimation variable to the specified object. Returns Success.")]
    public class SetSharedAnimationName : Action
    {
        [Tooltip("The value to set the SharedAnimationName to")]
        public SharedAnimationName targetValue;
        [RequiredField]
        [Tooltip("The SharedAnimationName to set")]
        public SharedAnimationName targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetValue = null;
            targetVariable = null;
        }
    }
}