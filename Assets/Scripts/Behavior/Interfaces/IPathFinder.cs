using Pathfinding;
using UnityEngine;

namespace CustomPathfinding
{
    public interface IPathFinder
    {
        public float speed { get; set; }
        public float rotationSpeed { get; set; }
        public bool rotateTowardsDestination { get; set; }
        public float stopDistance { get; set; }
        public Vector2 nextWaypoint { get; }
        public bool reachedDestination { get; }
        public Vector2 target { get; set; }
        public Path path {get;}
        public Rigidbody2D rb { get; }
        public void Stop();
    }
}
