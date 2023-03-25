using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    [TaskDescription("Seek the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/Custom2D")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class Seek : CustomAIMovement
    {
        [Tooltip("The GameObject that the agent is moving towards")]
        public SharedTransform target;
        [Tooltip("If target is null then use the target position")]
        public SharedVector2 targetPosition;

        public override void OnAwake()
        {
            base.OnAwake();
            SetDestination(Target());
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            SetDestination(Target());
            if (HasArrived())
            {
                return TaskStatus.Success;
            }
            
            if (agent.path != null && agent.path.error)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Running;
        }

        // Return targetPosition if target is null
        private Vector2 Target()
        {
            if (target.Value != null)
            {
                return target.Value.position;
            }
            return targetPosition.Value;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            targetPosition = Vector2.zero;
        }
    }
}