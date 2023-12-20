using BehaviorDesigner.Runtime.Tasks;
using Minigames.Fight;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPathfindingMovementController : MonoBehaviour
{
    private PlayerEntity _myEntity;
    private Seeker _seeker;
    private Rigidbody2D _myRigidbody2D;

    private Path path;
    private int currentWaypoint;
    private Vector3 targetPosition;
    private Direction lastDirection;

    private const float PATHFINDING_WAYPOINT_DISTANCE = .3f;

    void Start()
    {
        _myEntity = GetComponent<PlayerEntity>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();

        // find the chamber
        // TODO: use boss room to get access
        var chamber = FindObjectOfType<ConstructionChamber>();


        // path to the chamber
        _myEntity.IsControlled = true;
        _seeker = gameObject.AddComponent<Seeker>();
        _seeker.StartPath(transform.position, chamber.PlayerMoveTarget);
        _seeker.pathCallback += OnFinishPath;
    }

    private void OnFinishPath(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            targetPosition = path.vectorPath[0];
        }
    }

    private void OnDestroy()
    {
        _seeker.pathCallback -= OnFinishPath;
    }

    void FixedUpdate()
    {
        SetPathfindingTarget();

        Move();
    }

    private void SetPathfindingTarget()
    {
        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distanceToWaypoint < PATHFINDING_WAYPOINT_DISTANCE)
        {
            if (currentWaypoint + 1 < path.vectorPath.Count)
            {
                currentWaypoint++;
                targetPosition = path.vectorPath[currentWaypoint];
            }
            else
            {
                // send event when done
                Platform.EventService.Dispatch(new PlayerInteractedEvent(InteractionType.Craft));
            }
        }
    }

    private void Move()
    {
        var movement = targetPosition - transform.position;

        if (movement.magnitude > PATHFINDING_WAYPOINT_DISTANCE)
        {
            float speed = _myEntity.Stats.movementStats.moveSpeed.Calculated;

            Vector2 velocity = movement.normalized * speed * Time.fixedDeltaTime;
            velocity = Vector2.ClampMagnitude(velocity, movement.magnitude);

            Vector2 newPosition = (Vector2)transform.position + velocity;
            _myRigidbody2D.MovePosition(newPosition);

            Direction currentDirection = GetDirection(velocity);
            if(currentDirection != lastDirection)
            {
                Platform.EventService.Dispatch(new PlayerChangedDirectionEvent(currentDirection));
            }

            _myEntity.AnimationController.PlayRunAnimation();
            lastDirection = currentDirection;

        }
        else
        {
            _myRigidbody2D.velocity = Vector2.zero;
            Platform.EventService.Dispatch(new PlayerChangedDirectionEvent(Direction.Down));
            _myEntity.AnimationController.PlayIdleAnimation();
        }
    }

    private Direction GetDirection(Vector2 velocity)
    {
        if (velocity.y > 0)
        {
            return Direction.Up;
        }
        else if (velocity.y < 0)
        {
            return Direction.Down;
        }
        else if (velocity.x > 0)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.Left;
        }
    }
}
