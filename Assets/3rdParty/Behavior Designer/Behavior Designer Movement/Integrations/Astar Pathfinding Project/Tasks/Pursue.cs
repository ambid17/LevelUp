using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Pursue the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PursueIcon.png")]
    public class Pursue : IAstarAIMovement
    {
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The transform that the agent is pursuing")]
        public SharedGameObject target;

        // The position of the target at the last frame
        private Vector3 targetPosition;

        public override void OnStart()
        {
            base.OnStart();

            targetPosition = target.Value.transform.position;
            SetDestination(Target());
        }

        // Pursue the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            // Target will return the predicated position
            SetDestination(Target());

            if (HasArrived()) {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        // Predict the position of the target
        private Vector3 Target()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (target.Value.transform.position - transform.position).magnitude;
            var speed = Velocity().magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value) {
                futurePrediction = targetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = target.Value.transform.position;
            return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
            target = null;
        }
    }
}