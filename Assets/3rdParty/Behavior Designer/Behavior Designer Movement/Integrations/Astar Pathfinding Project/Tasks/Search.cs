using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Search for a target by combining the wander, within hearing range, and the within seeing range tasks.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SearchIcon.png")]
    public class Search : IAstarAIMovement
    {
        [Tooltip("Minimum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat minWanderDistance = 20;
        [Tooltip("Maximum distance ahead of the current position to look ahead for a destination")]
        public SharedFloat maxWanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 1;
        [Tooltip("The minimum length of time that the agent should pause at each destination")]
        public SharedFloat minPauseDuration = 0;
        [Tooltip("The maximum length of time that the agent should pause at each destination (zero to disable)")]
        public SharedFloat maxPauseDuration = 0;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 30;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("Should the search end if audio was heard?")]
        public SharedBool senseAudio = true;
        [Tooltip("How far away the unit can hear")]
        public SharedFloat hearingRadius = 30;
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("Specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 200;
        [Tooltip("Should the target bone be used?")]
        public SharedBool useTargetBone;
        [Tooltip("The target's bone if the target is a humanoid")]
        public HumanBodyBones targetBone;
        [Tooltip("Should a debug look ray be drawn to the scene view?")]
        public SharedBool drawDebugRay;
        [Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
                 "Set a threshold for the the minimum audibility level that the agent can hear")]
        public SharedFloat audibilityThreshold = 0.05f;
        [Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        private float pauseTime;
        private float destinationReachTime;

        private Collider[] overlapColliders;

        public override void OnStart()
        {
            base.OnStart();

            destinationReachTime = -pauseTime;
        }

        // Keep searching until an object is seen or heard (if senseAudio is enabled)
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

            // Detect if any objects are within sight
            if (overlapColliders == null) {
                overlapColliders = new Collider[maxCollisionCount];
            }
            returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, overlapColliders, objectLayerMask, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone, drawDebugRay.Value);
            // If an object was seen then return success
            if (returnedObject.Value != null) {
                return TaskStatus.Success;
            }
            // Detect if any object are within audio range (if enabled)
            if (senseAudio.Value) {
                returnedObject.Value = MovementUtility.WithinHearingRange(transform, offset.Value, audibilityThreshold.Value, hearingRadius.Value, overlapColliders, objectLayerMask);
                // If an object was heard then return success
                if (returnedObject.Value != null) {
                    return TaskStatus.Success;
                }
            }

            // No object has been seen or heard so keep searching
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
            base.OnReset();

            minWanderDistance = 20;
            maxWanderDistance = 20;
            wanderRate = 2;
            minPauseDuration = 0;
            maxPauseDuration = 0;
            fieldOfViewAngle = 90;
            viewDistance = 30;
            drawDebugRay = false;
            senseAudio = true;
            hearingRadius = 30;
            audibilityThreshold = 0.05f;
        }
    }
}