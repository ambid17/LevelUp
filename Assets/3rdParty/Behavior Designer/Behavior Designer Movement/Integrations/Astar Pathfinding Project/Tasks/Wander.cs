using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Wander using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
    public class Wander : IAstarAIMovement
    {
        [Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat minWanderDistance = 20;
        [Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat maxWanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 2;
        [Tooltip("The minimum length of time that the agent should pause at each destination")]
        public SharedFloat minPauseDuration = 0;
        [Tooltip("The maximum length of time that the agent should pause at each destination (zero to disable)")]
        public SharedFloat maxPauseDuration = 0;
        [Tooltip("The maximum number of retries per tick (set higher if using a slow tick time)")]
        public SharedInt targetRetries = 1;

        private float pauseTime;
        private float destinationReachTime;

        public override void OnStart()
        {
            base.OnStart();

            destinationReachTime = -pauseTime;
        }

        // There is no success or fail state with wander - the agent will just keep wandering
        public override TaskStatus OnUpdate()
        {
            if (!HasPath() || HasArrived()) {
                // The agent should pause at the destination only if the max pause duration is greater than 0
                if (maxPauseDuration.Value > 0) {
                    if (destinationReachTime == -1) {
                        destinationReachTime = Time.time;
                        pauseTime = Random.Range(minPauseDuration.Value, maxPauseDuration.Value);
                    }
                    if (destinationReachTime + pauseTime <= Time.time) {
                        // Only reset the time if a destination has been set.
                        if (TrySetTarget()) {
                            destinationReachTime = -1;
                        }
                    }
                } else {
                    TrySetTarget();
                }
            }
            return TaskStatus.Running;
        }

        private bool TrySetTarget()
        {
            var direction = transform.forward + Random.insideUnitSphere * wanderRate.Value;
            var destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
            SetDestination(SamplePosition(destination));
            return true;
        }

        // Reset the public variables
        public override void OnReset()
        {
            minWanderDistance = 20;
            maxWanderDistance = 20;
            wanderRate = 2;
            minPauseDuration = 0;
            maxPauseDuration = 0;
            targetRetries = 1;
        }
    }
}