namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("The priority task will assign a priority value to be accessed by the priority sequence selector " +
                     "the priority task will run until it's child returns success or failure and return the status of it's child.")]
    public class Priority : Decorator
    {
        [Tooltip("The priority value that this node should be sorted by")]
        public SharedFloat priority = 1;
        // The status of the child after it has finished running.
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override bool CanExecute()
        {
            // Auto fail if the priority is less than or equal to zero.
            if (priority.Value <= 0)
            {
                return false;
            }

            // Continue executing until the child task returns success or failure.
            return executionStatus == TaskStatus.Inactive || executionStatus == TaskStatus.Running;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Update the execution status after a child has finished running.
            executionStatus = childStatus;
        }

        public override float GetPriority()
        {
            // Return our priority value when called by selector.
            return priority.Value;
        }

        public override void OnEnd()
        {
            // Reset the execution status back to its starting values.
            executionStatus = TaskStatus.Inactive;
        }
    }
}