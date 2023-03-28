using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    [TaskDescription("Find a place to hide and move to it.")]
    [TaskCategory("Movement/Custom2D")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CoverIcon.png")]
    public class Cover : CustomAIMovement
    {
        [Tooltip("The distance to search for cover")]
        public SharedFloat maxCoverDistance = 1000;
        [Tooltip("Number of time to increment distance before reaching max distance")]
        public SharedInt maxCoverChecks = 3;
        [Tooltip("Number of cover points that can be found before we stop the search early")]
        public SharedInt maxCoverPoints = 5;
        [Tooltip("The distance behind a piece of cover that the raycast will be fired from")]
        public SharedFloat maxCoverThickness = 10;
        [Tooltip("The minimum distance one piece of cover has to be from another to be valid")]
        public SharedFloat minCoverSeparation = 5;
        [Tooltip("The layermask of the available cover positions")]
        public LayerMask availableLayerCovers;
        [Tooltip("The maximum number of raycasts that should be fired before the agent gives up looking for an agent to find cover behind")]
        public SharedInt maxRaycasts = 360;
        [Tooltip("How large the step should be between raycasts")]
        public SharedFloat rayStep = 1;
        [Tooltip("Once a cover point has been found, multiply this offset by the normal to prevent the agent from hugging the wall")]
        public SharedFloat coverOffset = 1;
        [Tooltip("Should the agent look at the cover point after it has arrived?")]
        public SharedBool lookAtCoverPoint = false;
        [Tooltip("The agent is done rotating to the cover point when the square magnitude is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Max rotation delta if lookAtCoverPoint")]
        public SharedFloat maxLookAtRotationDelta;

        // List of valid cover points
        private List<Vector2> coverPoints = new List<Vector2>();

        // The cover position
        private Vector3 coverPoint;
        private bool foundCover;

        public override void OnStart()
        {
            RaycastHit2D hit2D;
            int raycastCount = 0;

            //offset the direction by 45 degrees so that the first colliders found are in a vision cone in front of the player
            var direction = Quaternion.Euler(0, 0, transform.eulerAngles.z + 45) * Vector2.up;
            float step = 0;
            float checkIncrement = maxCoverDistance.Value / maxCoverChecks.Value;
            float distance = checkIncrement;
            var coverTarget = transform.position;

            // Safeguard
            coverPoints.Clear();

            // Keep doing raycast sweeps at incrementally longer distances until max number of points have been found or max distance has been reached
            while (distance < maxCoverDistance.Value)
            {
                // Keep firing a ray until too many rays have been fired
                while (raycastCount < maxRaycasts.Value)
                {
                    hit2D = Physics2D.Raycast(transform.position, direction, distance, availableLayerCovers.value);
                    if (hit2D)
                    {
                        // A suitable agent has been found. Find the opposite side of that agent by shooting a ray in the opposite direction from cover thickness away
                        hit2D = Physics2D.Raycast(hit2D.point - hit2D.normal * maxCoverThickness.Value, hit2D.normal, Mathf.Infinity);
                        if (hit2D)
                        {
                            // Calculate actual destination using normal and offset
                            coverTarget = hit2D.point + hit2D.normal * coverOffset.Value;
                            if (coverPoints.Count == 0 || IsValid(coverTarget))
                            {
                                coverPoint = hit2D.point;
                                // Add cover point to the list
                                coverPoints.Add(coverTarget);
                                foundCover = true;
                            }
                            // If max number of points have been reached then end the loop and set distance to max distance to end the process
                            if (coverPoints.Count - 1 >= maxCoverPoints.Value)
                            {
                                distance = maxCoverDistance.Value;
                                break;
                            }
                        }
                    }
                    // Keep sweeiping along the z axis
                    step += rayStep.Value;
                    direction = Quaternion.Euler(0, 0, transform.eulerAngles.z + 45 - step) * Vector2.up;
                    raycastCount++;
                }
                raycastCount = 0;
                distance += checkIncrement;
            }

            if (foundCover)
            {
                //select a random cover point from the list and pass it into the pathfinding script
                int i = Random.Range(0, coverPoints.Count);
                Vector2 pos = coverPoints[i];
                SetDestination(coverPoints[i]);
            }

            base.OnStart();
        }

        // Seek to the cover point. Return success as soon as the location is reached or the agent is looking at the cover point
        public override TaskStatus OnUpdate()
        {
            if (!foundCover)
            {
                return TaskStatus.Failure;
            }
            if (HasArrived())
            {
                var rotation = Quaternion.LookRotation(coverPoint - transform.position);
                // Return success if the agent isn't going to look at the cover point or it has completely rotated to look at the cover point
                if (!lookAtCoverPoint.Value || Quaternion.Angle(transform.rotation, rotation) < rotationEpsilon.Value)
                {
                    if (stopOnTaskEnd.Value)
                    {
                        Stop();
                    }
                    return TaskStatus.Success;
                }
                else
                {
                    // Still needs to rotate towards the target
                    // This doesn't work properly, will fix if we end up wanting to use it (their code)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxLookAtRotationDelta.Value);
                }
            }

            return TaskStatus.Running;
        }
        private bool IsValid(Vector2 point)
        {
            // Check distance from existing cover points against the min separation requirement
            foreach (Vector2 pos in coverPoints)
            {
                if (Vector2.Distance(point, pos) < minCoverSeparation.Value)
                {
                    return false;
                }
            }
            return true;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            maxCoverDistance = 1000;
            maxCoverChecks = 3;
            maxCoverPoints = 5;
            maxRaycasts = 360;
            maxCoverThickness = 10;
            minCoverSeparation = 5;
            rayStep = 1;
            coverOffset = 1;
            lookAtCoverPoint = false;
            rotationEpsilon = 0.5f;
        }
    }
}