using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerMovementController : MovementController
    {
        private Vector2 _movementToApply;
        private Vector2 _currentInput;
    
        void Start()
        {
            SetStartingMoveSpeed(GameManager.SettingsManager.playerSettings.MoveSpeed);
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
            MyRigidbody2D.velocity = _movementToApply;

            if (MyRigidbody2D.velocity.magnitude == 0)
            {
                MyEntity.VisualController.Animator.SetBool("IsMoving", false);
            }
            else
            {
                MyEntity.VisualController.Animator.SetBool("IsMoving", true);
            }
        
            //Flip the sprite based on velocity
            if(MyRigidbody2D.velocity.x < -0.1f) 
                MyEntity.VisualController.SpriteRenderer.flipX = false;
            else 
                MyEntity.VisualController.SpriteRenderer.flipX = true;
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
