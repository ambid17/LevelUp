using CustomPathfinding;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
public class BasicAIMovement : MonoBehaviour, IPathFinder
{
    // A* project script
    private Seeker seeker;
    private Rigidbody2D _rb;

    private float _Speed;
    private bool _RotateTowardsDestination;
    private const string updatePath = "UpdatePath";

    //track where we are in the waypoint array
    private int currentWaypoint;

    //A* project data type
    private Path _Path;
    private float _StopDistance;
    private Vector2 _Target;

    public float speed { get => _Speed;  set => _Speed = value;  }
    public bool rotateTowardsDestination { get => _RotateTowardsDestination;  set => _RotateTowardsDestination = value; }
    public float stopDistance { get => _StopDistance; set => _StopDistance = value; }
    public Vector2 target { get => _Target; set => _Target = value; }

    // Waypoint the agent is currently trying to reach
    public Vector2 nextWaypoint => path.vectorPath[currentWaypoint];
    // Destination has been reached if agent is within stopping distance of the final waypoint
    public bool reachedDestination => Vector2.Distance(transform.position, path.vectorPath[path.vectorPath.Count-1]) < stopDistance;
    // Path is invalid if the final waypoint is not within stopping distance of target
    public bool pathInvalid => path != null && Vector2.Distance(path.vectorPath[path.vectorPath.Count - 1], target) > stopDistance;
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
        // by default calculate a new path ever .5 seconds to adapt to target moving (causes slightly delayed pathing for fast moving objects but checking every frame causes jittering)
        InvokeRepeating(updatePath, 0, 0.5f);
    }
    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        Vector2 move = nextWaypoint - (Vector2)transform.position;
        rb.velocity = move.normalized * speed;
        float distance = Vector2.Distance(transform.position, nextWaypoint);
        if (distance < stopDistance)
        {
            if (currentWaypoint >= path.vectorPath.Count - 1)
            {
                UpdatePath();
                return;
            }
            currentWaypoint++;
        }
        if (rotateTowardsDestination)
        {
            //TODO rotation math
        }
    }
    public void UpdatePath()
    {
        // once seeker has finished calculating a path generate the path data
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }
    private void OnPathComplete(Path p)
    {
        // if path does not contain errors reset our current waypoint and store the path data
        if (!p.error)
        {
            _Path = p;
            currentWaypoint = 0;
        }
    }

    public void Stop()
    {
        target = transform.position;
        rb.velocity = Vector2.zero;
        speed = 0;
        _Path = null;
    }
}
