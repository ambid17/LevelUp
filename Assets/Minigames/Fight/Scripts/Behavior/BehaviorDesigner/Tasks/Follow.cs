using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    [TaskDescription("Follow the target specified using AIPath.")]
    [TaskCategory("Movement/Custom2D")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FollowIcon.png")]
    public class Follow : CustomAIMovement
    {
        [Tooltip("The GameObject that the agent is following")]
        public SharedTransform target;
        [Tooltip("Start moving towards the target if the target is further than the specified distance")]
        public SharedFloat moveDistance = 2;

        private Vector2 lastTargetPosition;
        private bool hasMoved;

        public override void OnStart()
        {
            base.OnStart();

            lastTargetPosition = (Vector2)target.Value.position + Vector2.one * (moveDistance.Value + 1);
            hasMoved = false;
        }

        // Follow the target. The task will never return success as the agent should continue to follow the target even after arriving at the destination.
        public override TaskStatus OnUpdate()
        {
            // Move if the target has moved more than the moveDistance since the last time the agent moved.
            var targetPosition = (Vector2)target.Value.transform.position;
            if ((targetPosition - lastTargetPosition).magnitude >= moveDistance.Value)
            {
                SetDestination(targetPosition);
                lastTargetPosition = targetPosition;
                hasMoved = true;
            }
            else
            {
                // Stop moving if the agent is within the moveDistance of the target.
                if (hasMoved && ((Vector3)targetPosition - transform.position).magnitude < moveDistance.Value)
                {
                    Stop();
                    hasMoved = false;
                    lastTargetPosition = targetPosition;
                }
            }
            // If agent reaches the end of the path but can't reach the final destination return failure
            if (HasArrived() && agent.pathInvalid)
            {
                Stop();
                return TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            moveDistance = 2;
        }
    }
}