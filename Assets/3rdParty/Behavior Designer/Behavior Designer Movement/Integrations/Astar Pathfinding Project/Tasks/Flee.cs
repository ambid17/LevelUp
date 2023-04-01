using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Flee the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FleeIcon.png")]
    public class Flee : IAstarAIMovement
    {
        [Tooltip("The agent has fleed when the magnitude is greater than this value")]
        public SharedFloat fleedDistance = 20;
        [Tooltip("The distance to look ahead when fleeing")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("The GameObject that the agent is fleeing from")]
        public SharedGameObject target;

        // Flee from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            if (Vector3.Magnitude(transform.position - target.Value.transform.position) > fleedDistance.Value) {
                return TaskStatus.Success;
            }

            SetDestination(Target());
            return TaskStatus.Running;
        }

        // Flee in the opposite direction
        private Vector3 Target()
        {
            return transform.position + (transform.position - target.Value.transform.position).normalized * lookAheadDistance.Value;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            fleedDistance = 20;
            lookAheadDistance = 5;
            target = null;
        }
    }
}