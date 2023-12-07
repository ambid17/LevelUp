    using CustomPathfinding;
using Pathfinding;
using System.Collections;
using UnityEngine;

namespace Minigames.Fight
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicAIMovement : MonoBehaviour, IPathFinder
    {
        [SerializeField]
        private Entity entity;

        // A* project script
        private Seeker seeker;
        private Rigidbody2D _rb;

        private float _Speed;
        private bool _RotateTowardsDestination;
        private float _rotationSpeed;

        //track where we are in the waypoint array
        private int currentWaypoint;

        //A* project data type
        private Path _Path;
        private float _StopDistance;
        private Vector2 _Target;

        private float _lastKnownLookDirection;
        int framesSinceLastDirectionChange;

        [SerializeField]
        private Vector2 move;

        public float speed { get => _Speed; set => _Speed = value; }
        public bool rotateTowardsDestination { get => _RotateTowardsDestination; set => _RotateTowardsDestination = value; }
        public float rotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        public float stopDistance { get => _StopDistance; set => _StopDistance = value; }
        public Vector2 target { get => _Target; set => _Target = value; }

        // Waypoint the agent is currently trying to reach
        public Vector2 nextWaypoint => path.vectorPath[currentWaypoint];

        // Destination has been reached if agent is within stopping distance of the final waypoint
        public bool reachedDestination => Vector2.Distance(transform.position, target) < stopDistance;

        public bool pathInvalid
        {
            get
            {
                NNConstraint myConstraint = NNConstraint.None;
                myConstraint.graphMask = seeker.graphMask;
                GraphNode node1 = AstarPath.active.GetNearest(transform.position, myConstraint).node;
                GraphNode node2 = AstarPath.active.GetNearest(target, myConstraint).node;
                return !PathUtilities.IsPathPossible(node1, node2);
            }
        }
        public Path path => _Path;
        public Rigidbody2D rb => _rb;

        private void Awake()
        {
            seeker = GetComponent<Seeker>();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            // idiot proofing
            rb.gravityScale = 0;
            StartCoroutine(RepeatUpdatePath());
        }
        private IEnumerator RepeatUpdatePath()
        {
            while (true)
            {
                UpdatePath();
                yield return new WaitForSeconds(0.5f);
            }
        }
        private void FixedUpdate()
        {
            // If we don't have a path moving is bad
            if (path == null)
            {
                return;
            }
            if (entity.Stunned)
            {
                Stop();
                return;
            }
            float distance = Mathf.Infinity;
            if (seeker.IsDone())
            {
                move = nextWaypoint - transform.position.AsVector2();
                distance = Vector2.Distance(transform.position, nextWaypoint);
            }

            // If our path is invalid we want to move to the nearest walkable node to avoid getting stuck.
            if (pathInvalid)
            {
                NNConstraint myConstraint = NNConstraint.None;
                myConstraint.graphMask = seeker.graphMask;
                Vector2 nearestWalkable = (Vector3)AstarPath.active.GetNearest(transform.position, myConstraint).node.position;

                // Multiplied by 2 to avoid getting in an infinite back and forth loop (spooky magic float).
                move = nearestWalkable *2f - transform.position.AsVector2();
            }

            // If waypoint has been reached then agent heads towards next waypoint on the list
            // If no other waypoints exist then agent recalculates the path

            // If stopDistance is set to 0 then update waypoint at 0.01 instead so that agent can still move.
            float distanceToUpdateWaypoint = stopDistance;
            if (distanceToUpdateWaypoint == 0)
            {
                distanceToUpdateWaypoint = 0.01f;
            }
            if (distance <= distanceToUpdateWaypoint)
            {
                if (currentWaypoint >= path.vectorPath.Count - 1)
                {
                    // TODO from sabien: delete? this seems incorrect
                    UpdatePath();
                    // Kill off our velocity while we are waiting
                    rb.velocity = Vector2.zero;
                    return;
                }
                currentWaypoint++;
            }

            rb.velocity = move.normalized * speed;

            // To prevent sprite from flickering wait until the direction has changed for at least 5 physics updates in a row before switching
            if (Mathf.Abs(move.x) > 0.01)
            {
                if (Mathf.Sign(_lastKnownLookDirection) != Mathf.Sign(move.x))
                {
                    framesSinceLastDirectionChange++;
                    if (framesSinceLastDirectionChange >= 5)
                    {
                        framesSinceLastDirectionChange = 0;
                        _lastKnownLookDirection = move.x;
                    }   
                }
                else
                {
                    framesSinceLastDirectionChange = 0;
                }
            }

            entity.VisualController.SpriteRenderer.flipX = _lastKnownLookDirection < 0 ? false : true;

            
        }
        private void OnPathComplete(Path p)
        {
            // if path does not contain errors reset our current waypoint and store the path data
            if (!p.error)
            {
                _Path = p;
                currentWaypoint = 1;
            }
        }
        private void UpdatePath()
        {
            if (Vector2.Distance(transform.position, target) <= stopDistance)
            {
                return;
            }
            // once seeker has finished calculating a path generate the path data
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target, OnPathComplete);
            }
        }

        public void Stop()
        {
            target = transform.position;
            rb.velocity = Vector2.zero;
            _Path = null;
        }
    }

}