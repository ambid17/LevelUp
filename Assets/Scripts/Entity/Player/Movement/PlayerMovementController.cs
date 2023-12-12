using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerMovementController : MonoBehaviour
    {
        private Vector2 _movementToApply;
        private Vector2 _currentInput;

        private EventService _eventService;

        private const float _idleSpeed = .1f;

        [SerializeField]
        private PlayerEntity _myEntity;
        public Rigidbody2D MyRigidbody2D;

        private Vector2 _lastInput;
        private const float ACCELERATION = 20;
    
        void Start()
        {
            _eventService = Platform.EventService;
            MyRigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (GameManager.PlayerEntity.IsDead || GameManager.PlayerEntity.Stunned)
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
            _currentInput = input.normalized * _myEntity.Stats.movementStats.moveSpeed.Calculated;
        }
    
        private void Move()
        {
            if (MyRigidbody2D.velocity.sqrMagnitude <= _idleSpeed)
            {
                _myEntity.AnimationController.PlayIdleAnimation();
                return;
            }
            _myEntity.AnimationController.PlayRunAnimation();
        }
    
        private void FixedUpdate()
        {
            if (GameManager.PlayerEntity.IsDead || GameManager.PlayerEntity.Stunned)
            {
                MyRigidbody2D.velocity = Vector2.zero;
                return;
            }
            ApplyAcceleration();
        }
    
        private void ApplyAcceleration()
        {
            float maxAcceleration = ACCELERATION * Time.fixedDeltaTime;
            _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, maxAcceleration);
            _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, maxAcceleration);
            MyRigidbody2D.velocity = _movementToApply;
        }
    }
}
