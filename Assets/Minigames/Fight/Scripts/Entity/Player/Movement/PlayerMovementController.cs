using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerMovementController : MovementController
    {
        private Vector2 _movementToApply;
        private Vector2 _currentInput;

        private EventService _eventService;

        private float idleSpeed = .1f;

        PlayerEntity _myEntity;

        private Vector2 _lastInput;
    
        void Start()
        {
            SetStartingMoveSpeed(GameManager.SettingsManager.playerSettings.MoveSpeed);
            _eventService = GameManager.EventService;
            _myEntity = MyEntity as PlayerEntity;
        }

        void Update()
        {
            if (GameManager.PlayerEntity.IsDead || !GameManager.PlayerEntity.CanMove)
            {
                MyRigidbody2D.velocity = Vector2.zero;
                return;
            }
        
            GetMovementInput();
            Move();
        }

        private void GetMovementInput()
        {
            Vector2 input = Vector2.zero;
            Direction direction = Direction.Down;
        
            if (Input.GetKey(KeyCode.W))
            {
                input.y += 1;
                direction = Direction.Up;
            }
        
            if (Input.GetKey(KeyCode.S))
            {
                input.y -= 1;
                direction = Direction.Down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
                direction = Direction.Right;
            }

            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
                direction = Direction.Left;
            }

            // No need to dispatch the event unless our direction has changed.
            if (_lastInput != input && input != Vector2.zero)
            {
                _eventService.Dispatch(new PlayerChangedDirectionEvent(direction));
            }

            _lastInput = input;
            _currentInput = input.normalized * CurrentMoveSpeed;
        }
    
        private void Move()
        {
            MyRigidbody2D.velocity = _movementToApply;

            if (MyRigidbody2D.velocity.sqrMagnitude <= idleSpeed)
            {
                _myEntity.AnimationController.PlayIdleAnimation();
                return;
            }
            _myEntity.AnimationController.PlayRunAnimation();
        }
    
        private void FixedUpdate()
        {
            ApplyAcceleration();
        }
    
        private void ApplyAcceleration()
        {
            float maxAcceleration = GameManager.SettingsManager.playerSettings.Acceleration * Time.fixedDeltaTime;
            _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, maxAcceleration);
            _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, maxAcceleration);
        }
    }
}
