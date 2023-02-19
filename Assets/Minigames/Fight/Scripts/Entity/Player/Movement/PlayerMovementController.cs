using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerMovementController : MovementController
    {
        private Rigidbody2D _rigidbody2D;
    
        private Vector2 _movementToApply;
        private Vector2 _currentInput;
        
        private VisualController _visualController;
    
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _visualController = GameManager.PlayerEntity.VisualController;
            SetStartingMoveSpeed(GameManager.SettingsManager.playerSettings.MoveSpeed);
        }

        void Update()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }
        
            GetMovementInput();
            Move();
        }

        private void GetMovementInput()
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }
        
            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }
        
            if (Input.GetKey(KeyCode.W))
            {
                input.y += 1;
            }
        
            if (Input.GetKey(KeyCode.S))
            {
                input.y -= 1;
            }
        
            _currentInput = input.normalized * CurrentMoveSpeed;
        }
    
        private void Move()
        {
            _rigidbody2D.velocity = _movementToApply;

            if (_rigidbody2D.velocity.magnitude == 0)
            {
                _visualController.animator.SetBool("IsMoving", false);
            }
            else
            {
                _visualController.animator.SetBool("IsMoving", true);
            }
        
            //Flip the sprite based on velocity
            if(_rigidbody2D.velocity.x < -0.1f) 
                _visualController.spriteRenderer.flipX = false;
            else 
                _visualController.spriteRenderer.flipX = true;
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
