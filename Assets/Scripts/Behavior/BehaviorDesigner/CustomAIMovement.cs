using CustomPathfinding;
using Pathfinding;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    public abstract class CustomAIMovement : Movement
    {
        [Tooltip("the speed of the agent")]
        public SharedFloat speed = 5;
        [Tooltip("should the agent stop moving when the task ends")]
        public SharedBool stopOnTaskEnd = false;
        [Tooltip("distance the agent must come within to have reached it's destination")]
        public SharedFloat stopDistance = 1;
        [Tooltip("should the agent rotate towards it's next waypoint OR rotate to face its target")]
        public SharedBool rotateTowardsDestination = true;
        [Tooltip("speed of agent rotation smoothing")]
        public SharedFloat rotationSpeed = 1;

        protected IPathFinder agent;

        public override void OnAwake()
        {
            agent = gameObject.GetComponent<IPathFinder>();
        }
        public override void OnStart()
        {
            agent.speed = speed.Value;
            agent.rotateTowardsDestination = rotateTowardsDestination.Value;
            agent.rotationSpeed = rotationSpeed.Value;
            agent.stopDistance = stopDistance.Value;
        }
        protected override bool SetDestination(Vector3 target)
        {
            agent.target = (Vector2)target;
            return true;
        }
        protected override Vector3 Velocity()
        {
            return agent.rb.velocity;
        }
        protected override void UpdateRotation(bool update)
        {
            //inhereted but not needed
        }
        protected override bool HasPath()
        {
            return agent.path.IsDone();
        }
        protected override void Stop()
        {
            agent.Stop();
        }
        protected override bool HasArrived()
        {
            return agent.reachedDestination;
        }
        public override void OnReset()
        {
            speed = 5;
            stopOnTaskEnd = false;
            stopDistance = 1;
            rotateTowardsDestination = true;
        }
    }
}
