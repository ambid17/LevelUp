using Unity.VisualScripting;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Find a place to hide and move to it.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CoverIcon.png")]
    public class Cover : IAstarAIMovement
    {
        [Tooltip("The distance to search for cover")]
        public SharedFloat maxCoverDistance = 1000;
        [Tooltip("The distance behind a piece of cover that the raycast will be fired from")]
        public SharedFloat maxCoverThickness = 10;
        [Tooltip("The layermask of the available cover positions")]
        public LayerMask availableLayerCovers;
        [Tooltip("The maximum number of raycasts that should be fired before the agent gives up looking for an agent to find cover behind")]
        public SharedInt maxRaycasts = 100;
        [Tooltip("How large the step should be between raycasts")]
        public SharedFloat rayStep = 1;
        [Tooltip("Once a cover point has been found, multiply this offset by the normal to prevent the agent from hugging the wall")]
        public SharedFloat coverOffset = 2;
        [Tooltip("Should the agent look at the cover point after it has arrived?")]
        public SharedBool lookAtCoverPoint = false;
        [Tooltip("The agent is done rotating to the cover point when the square magnitude is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Max rotation delta if lookAtCoverPoint")]
        public SharedFloat maxLookAtRotationDelta;

        // The cover position
        private Vector3 coverPoint;
        private bool foundCover;

        public override void OnStart()
        {
            RaycastHit hit;
            RaycastHit2D hit2D;
            int raycastCount = 0;
            var direction = use2DMovement ? transform.up : transform.forward;
            float step = 0;
            float coverDistance = Mathf.Infinity;
            var coverTarget = transform.position;
            if (use2DMovement)
            {
                // Keep firing a ray until too many rays have been fired
                while (raycastCount < maxRaycasts.Value)
                {
                    hit2D = Physics2D.Raycast(transform.position, direction, maxCoverDistance.Value, availableLayerCovers.value);
                    if (hit2D)
                    {
                        // A suitable agent has been found. Find the opposite side of that agent by shooting a ray in the opposite direction from a point far away
                        hit2D = Physics2D.Raycast(hit2D.point - hit2D.normal * maxCoverThickness.Value, hit2D.normal, Mathf.Infinity);
                        if (hit2D)
                        {
                            if (Vector2.Distance(transform.position, hit2D.point) < coverDistance)
                            {
                                coverDistance = Vector2.Distance(transform.position, hit2D.point);
                                coverPoint = hit2D.point;
                                coverTarget = hit2D.point + hit2D.normal * coverOffset.Value;
                                foundCover = true;
                            }
                        }
                    }
                    // Keep sweeiping along the y axis
                    step += rayStep.Value;
                    direction = Quaternion.Euler(0, 0, transform.eulerAngles.z + step) * Vector2.up;
                    raycastCount++;
                }
            }
            else
            {
                // Keep firing a ray until too many rays have been fired
                while (raycastCount < maxRaycasts.Value)
                {
                    var ray = new Ray(transform.position, direction);
                    if (Physics.Raycast(ray, out hit, maxCoverDistance.Value, availableLayerCovers.value))
                    {
                        // A suitable agent has been found. Find the opposite side of that agent by shooting a ray in the opposite direction from a point far away
                        if (hit.collider.Raycast(new Ray(hit.point - hit.normal * maxCoverThickness.Value, hit.normal), out hit, Mathf.Infinity))
                        {
                            if (Vector3.Distance(transform.position, hit.point) < coverDistance)
                            {
                                coverDistance = Vector3.Distance(transform.position, hit.point);
                                coverPoint = hit.point;
                                coverTarget = hit.point + hit.normal * coverOffset.Value;
                                foundCover = true;
                            }
                        }
                    }
                    // Keep sweeiping along the y axis
                    step += rayStep.Value;
                    direction = Quaternion.Euler(0, transform.eulerAngles.y + step, 0) * Vector3.forward;
                    raycastCount++;
                }
            }

            if (foundCover) {
                SetDestination(coverTarget);
            }

            base.OnStart();
        }

        // Seek to the cover point. Return success as soon as the location is reached or the agent is looking at the cover point
        public override TaskStatus OnUpdate()
        {
            if (!foundCover) {
                return TaskStatus.Failure;
            }
            if (HasArrived()) {
                var rotation = Quaternion.LookRotation(coverPoint - transform.position);
                // Return success if the agent isn't going to look at the cover point or it has completely rotated to look at the cover point
                if (!lookAtCoverPoint.Value || Quaternion.Angle(transform.rotation, rotation) < rotationEpsilon.Value) {
                    return TaskStatus.Success;
                } else {
                    // Still needs to rotate towards the target
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxLookAtRotationDelta.Value);
                }
            }

            return TaskStatus.Running;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            maxCoverDistance = 1000;
            maxRaycasts = 100;
            rayStep = 1;
            coverOffset = 2;
            lookAtCoverPoint = false;
            rotationEpsilon = 0.5f;
        }
    }
}