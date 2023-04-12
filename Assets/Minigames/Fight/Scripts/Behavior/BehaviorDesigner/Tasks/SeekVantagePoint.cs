using Pathfinding;
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
        [Tooltip("Should the agent require vantage point to have line of sight?")]
        public SharedBool requireLineOfSight = true;
        [Tooltip("Layermask for line of sight check.")]
        public SharedLayerMask obstacleLayerMask;


        // Store the node the target is nearest to so we can determine when it enters another node.
        private GraphNode _currentTargetNode;

        public override void OnAwake()
        {
            base.OnAwake();
            FindNewVantagePoint();
        }
        public override TaskStatus OnUpdate()
        {
            // Get the nearest walkable node to the target point.
            GraphNode nearestWalkableNode = AstarPath.active.GetNearest(target.Value.position, NNConstraint.Default).node;

            // If the nearest walkable node to the target has changed then run vantage point check again.
            if (nearestWalkableNode != _currentTargetNode)
            {
                FindNewVantagePoint();
            }
            if (HasArrived())
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        private void FindNewVantagePoint()
        {
            List<GraphNode> vantagePoints = PathUtilities.GetReachableNodesWithinRadius(target.Value.position, radius.Value);

            // If line of sight is required remove any nodes that don't have line of sight to the target.
            if (requireLineOfSight.Value)
            {
                foreach (GraphNode vantagePoint in vantagePoints)
                {
                    Vector2 direction = target.Value.position - (Vector3)vantagePoint.position;
                    RaycastHit2D hit = Physics2D.Raycast((Vector3)vantagePoint.position, direction.normalized, radius.Value, obstacleLayerMask.Value);
                    if (hit)
                    {
                        vantagePoints.Remove(vantagePoint);
                    }
                }
            }

            // Find the node that is closest to the agent.
            float minDistance = Mathf.Infinity;
            Vector2 targetPos = transform.position;
            foreach (GraphNode vantagePoint in vantagePoints)
            {
                if (Vector2.Distance(transform.position, (Vector3)vantagePoint.position) < minDistance)
                {
                    minDistance = Vector2.Distance(transform.position, (Vector3)vantagePoint.position);
                    targetPos = (Vector3)vantagePoint.position;
                    _currentTargetNode = vantagePoint;
                }
            }

            SetDestination(targetPos);
        }
        public override void OnReset()
        {
            base.OnReset();

            radius = 10;
            requireLineOfSight = true;
        }
    }
}