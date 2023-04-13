using CustomPathfinding;
using Pathfinding;
using System.Collections;
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
    private float _rotationSpeed;
    private const string updatePath = "UpdatePath";
    private bool _lastCalculatedPathInvalid;

    //track where we are in the waypoint array
    private int currentWaypoint;

    //A* project data type
    private Path _Path;
    private float _StopDistance;
    private Vector2 _Target;

    public float speed { get => _Speed;  set => _Speed = value;  }
    public bool rotateTowardsDestination { get => _RotateTowardsDestination;  set => _RotateTowardsDestination = value; }
    public float rotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    public float stopDistance { get => _StopDistance; set => _StopDistance = value; }
    public Vector2 target { get => _Target; set => _Target = value; }

    // Waypoint the agent is currently trying to reach
    public Vector2 nextWaypoint => path.vectorPath[currentWaypoint];

    // Destination has been reached if agent is within stopping distance of the final waypoint
    public bool reachedDestination => Vector2.Distance(transform.position, path.vectorPath[path.vectorPath.Count-1]) < stopDistance;

    public bool pathInvalid
    {
        get
        {
            GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
            GraphNode node2 = AstarPath.active.GetNearest(target, NNConstraint.Default).node;
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
        Vector2 move = nextWaypoint - (Vector2)transform.position;
        rb.velocity = move.normalized * speed;
        float distance = Vector2.Distance(transform.position, nextWaypoint);
        // If waypoint has been reached then agent heads towards next waypoint on the list
        // If no other waypoints exist then agent recalculates the path
        if (distance <= stopDistance)
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
        if (rotateTowardsDestination)
        {
            transform.rotation = PhysicsUtils.LookAt(transform, nextWaypoint, rotationSpeed * Time.deltaTime);
        }
    }
    private void OnPathComplete(Path p)
    {
        // if path does not contain errors reset our current waypoint and store the path data
        if (!p.error)
        {
            _Path = p;
            currentWaypoint = 0;
            _lastCalculatedPathInvalid = path != null && Vector2.Distance(path.vectorPath[path.vectorPath.Count - 1], target) > stopDistance;
        }
    }
    private void UpdatePath()
    {
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
        speed = 0;
        _Path = null;
    }
}
