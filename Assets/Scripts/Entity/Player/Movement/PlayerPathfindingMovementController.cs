using BehaviorDesigner.Runtime.Tasks;
using FunkyCode.Utilities;
using Minigames.Fight;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerPathfindingMovementController : MonoBehaviour
    {
        private PlayerEntity _myEntity;
        private Seeker _seeker;
        private SimpleSmoothModifier _simpleSmoothModifier;
        private Rigidbody2D _myRigidbody2D;
        private PlayerControlledActionType _actionType;

        private Path path;
        private int currentWaypoint;
        private Vector3 targetPosition;
        private Direction lastDirection;

        private const float REACHED_DESTINATION_DISTANCE = .01f;
        private const float CINEMATIC_MOVE_SPEED = 5;

        public void StartPath(Vector2 target, PlayerControlledActionType actionType)
        {
            _myEntity = GetComponent<PlayerEntity>();
            _myRigidbody2D = GetComponent<Rigidbody2D>();
            _myRigidbody2D.velocity = Vector2.zero;
            // path to the chamber
            _myEntity.IsControlled = true;
            _seeker = gameObject.AddComponent<Seeker>();
            _simpleSmoothModifier = gameObject.AddComponent<SimpleSmoothModifier>();
            _seeker.graphMask = GraphMask.FromGraphName("PlayerGraph");
            _seeker.StartPath(transform.position, target, OnFinishPath);
            _actionType = actionType;
        }

        private void OnFinishPath(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 2;
                targetPosition = path.vectorPath[currentWaypoint];
            }
        }

        private void OnDestroy()
        {
            _seeker.pathCallback -= OnFinishPath;
        }

        void Update()
        {
            if (path == null || !_seeker.IsDone())
            {
                // We have no path to follow yet, so don't do anything
                return;
            }

            SetPathfindingTarget();
            Move();
        }

        private void SetPathfindingTarget()
        {
            Vector3 currentPosition = transform.position;
            currentPosition.z = 0;

            Vector3 currentTarget = path.vectorPath[currentWaypoint];
            currentTarget.z = 0;

            float distanceToWaypoint = Vector3.Distance(currentPosition, currentTarget);

            if (distanceToWaypoint < REACHED_DESTINATION_DISTANCE)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                    targetPosition = path.vectorPath[currentWaypoint];
                }
                else
                {
                    Platform.EventService.Dispatch(new PlayerControlledActionFinishedEvent(_actionType));
                    Destroy(_simpleSmoothModifier);
                    Destroy(_seeker);
                    Destroy(this);
                }
            }
        }

        private void Move()
        {
            var offsetToWaypoint = targetPosition - transform.position;
            offsetToWaypoint.z = 0;

            if (offsetToWaypoint.magnitude > REACHED_DESTINATION_DISTANCE)
            {
                float speedThisFrame = CINEMATIC_MOVE_SPEED * Time.deltaTime;
                Vector2 velocityBeforeClamp = offsetToWaypoint.normalized * speedThisFrame;
                // clamp the new velocity length to the distance to the next waypoint so we can't overshoot
                Vector2 velocity = Vector2.ClampMagnitude(velocityBeforeClamp, offsetToWaypoint.magnitude);
                Vector2 newPosition = transform.position.AsVector2() + velocity;

                transform.position = newPosition;

                Direction currentDirection = GetDirection(velocity);
                if (currentDirection != lastDirection)
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
}