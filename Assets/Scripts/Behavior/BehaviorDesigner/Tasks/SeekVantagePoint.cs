using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    [TaskDescription("Locate a vantage point within a radius of the target.")]
    [TaskCategory("Movement/Custom2D")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class SeekVantagePoint : CustomAIMovement
    {
        [Tooltip("The target to seek a vantage point on.")]
        public SharedTransform target;
        [Tooltip("The radius around the target to seek.")]
        public SharedFloat radius = 10f;
        [Tooltip("How far past the target the agent should offset to ensure line of sight")]
        public SharedFloat offset = .1f;
        [Tooltip("Should the agent require vantage point to have line of sight?")]
        public SharedBool requireLineOfSight = true;
        [Tooltip("Layermask for line of sight check.")]
        public SharedLayerMask obstacleLayerMask;


        // Store the node the target is nearest to so we can determine when it enters another node.
        private GraphNode _currentTargetNode;

        // Store the position after calculating offset.
        private Vector2 offsetTarget;

        private bool _isDoneFindingPoint;

        public override void OnAwake()
        {
            base.OnAwake();
            StartCoroutine(FindNewVantagePoint());
        }
        public override TaskStatus OnUpdate()
        {
            // Get the nearest walkable node to the target point.
            GraphNode nearestWalkableNode = AstarPath.active.GetNearest(target.Value.position, NNConstraint.Default).node;

            // If the nearest walkable node to the target has changed then run vantage point check again.
            if (nearestWalkableNode != _currentTargetNode && _isDoneFindingPoint)
            {
                StartCoroutine(FindNewVantagePoint());
            }
            if (HasArrived())
            {
                return HasReachedOffset();
            }
            if (_isDoneFindingPoint)
            {
                OffsetTarget();
            }
            return TaskStatus.Running;
        }

        private IEnumerator FindNewVantagePoint()
        {
            _isDoneFindingPoint = false;
            List<GraphNode> oldVantagePoints = PathUtilities.GetReachableNodesWithinRadius(target.Value.position, radius.Value);

            List<GraphNode> newVantagePoints = new List<GraphNode>();

            // If line of sight is required remove any nodes that don't have line of sight to the target.
            if (requireLineOfSight.Value)
            {
                foreach (GraphNode vantagePoint in oldVantagePoints)
                {
                    Vector2 direction = target.Value.position - (Vector3)vantagePoint.position;
                    RaycastHit2D hit = Physics2D.Raycast((Vector3)vantagePoint.position, direction, radius.Value, obstacleLayerMask.Value);
                    if (!hit)
                    {
                        newVantagePoints.Add(vantagePoint);
                    }
                    yield return new WaitForSeconds(0);
                }
            }

            // Find the node that is closest to the agent.
            float minDistance = Mathf.Infinity;
            Vector2 targetPos = transform.position;
            foreach (GraphNode vantagePoint in newVantagePoints)
            {
                if (Vector2.Distance(transform.position, (Vector3)vantagePoint.position) < minDistance)
                {
                    minDistance = Vector2.Distance(transform.position, (Vector3)vantagePoint.position);
                    targetPos = (Vector3)vantagePoint.position;
                    _currentTargetNode = vantagePoint;
                }
            }

            SetDestination(targetPos);
            _isDoneFindingPoint = true;
        }

        // Calculate offset to ensure Line of Sight.
        private void OffsetTarget()
        {
            Vector2 difference = (Vector3)_currentTargetNode.position - transform.position;
            float distance = difference.magnitude;
            Vector2 direction = difference.normalized;

            offsetTarget = direction * (distance + offset.Value);
        }
        private TaskStatus HasReachedOffset()
        {
            return Vector2.Distance(transform.position, offsetTarget) < stopDistance.Value ? TaskStatus.Running : TaskStatus.Success;
        }
        public override void OnReset()
        {
            base.OnReset();

            radius = 10;
            requireLineOfSight = true;
        }
    }
}