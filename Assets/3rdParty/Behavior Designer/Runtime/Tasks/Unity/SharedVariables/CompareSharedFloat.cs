namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareSharedFloat : Conditional
    {
        [Tooltip("The first variable to compare")]
        public SharedFloat variable;
        [Tooltip("The variable to compare to")]
        public SharedFloat compareTo;

        public Comparison comparisonType;
        public enum Comparison
        {
            LessThan,
            GreaterThan,
            EqualTo,
            LessThanOrEqualTo,
            GreaterThanOrEqualTo,
        }

        public override TaskStatus OnUpdate()
        {
            switch (comparisonType)
            {
                case Comparison.LessThan:
                    return variable.Value < compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Comparison.GreaterThan:
                    return variable.Value > compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Comparison.EqualTo:
                    return variable.Value == compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Comparison.LessThanOrEqualTo:
                    return variable.Value <= compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Comparison.GreaterThanOrEqualTo:
                    return variable.Value >= compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            variable = 0;
            compareTo = 0;
        }
    }
}