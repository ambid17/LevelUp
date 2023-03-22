using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    // Abstract class for any task that uses an IAstarAI cmponent
    public abstract class IAstarAIMovement : Movement
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("Should the NavMeshAgent be stopped when the task ends?")]
        public SharedBool stopOnTaskEnd = true;
        [Tooltip("Is the agent being used for 2D movement?")]
        public bool use2DMovement;

        protected IAstarAI agent;

        public override void OnAwake()
        {
            agent = gameObject.GetComponent<IAstarAI>();
        }

        public override void OnStart()
        {
            agent.maxSpeed = speed.Value;
        }

        protected override bool SetDestination(Vector3 target)
        {
            if (agent.pathPending) return true;

            agent.canSearch = true;
            agent.canMove = true;
            agent.destination = target;
            agent.SearchPath();
            return true;
        }

        protected override Vector3 Velocity()
        {
            return agent.velocity;
        }

        protected override void UpdateRotation(bool update)
        {
            // Intentionally left blank
        }

        protected Vector3 SamplePosition(Vector3 position)
        {
            return (Vector3)AstarPath.active.GetNearest(position).node.position;
        }

        protected override bool HasPath()
        {
            return agent.hasPath;
        }

        protected override void Stop()
        {
            agent.destination = transform.position;
            agent.canMove = false;
            agent.canSearch = false;
            agent.SetPath(null);
        }

        protected override bool HasArrived()
        {
            return !agent.pathPending && (agent.reachedEndOfPath || !agent.hasPath);
        }

        public override void OnEnd()
        {
            if (stopOnTaskEnd.Value) {
                Stop();
            }
        }

        public override void OnReset()
        {
            speed = 10;
            stopOnTaskEnd = true;
        }
    }
}
