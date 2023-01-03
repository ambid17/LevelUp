using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Mining
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _currentInput;
        private Camera _camera;
        private SpriteRenderer _spriteRenderer;
        private Vector2 velocity;
        [SerializeField] private float acceleration, jetpackAcceleration;
        private bool canDig = true;
        private bool isGrounded;
        [SerializeField] private float dragCoefficient;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float maxJetpackSpeed;
        [SerializeField] private float digRange;
        [SerializeField] private ContactFilter2D digContactFilter;
        private Rigidbody2D _rb;


        public void SetGrounded(bool g)
        {
            isGrounded = g;
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            GetMovement();
            Move();
            TryDig();
        }

        void GetMovement()
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
            _currentInput.x = input.x * acceleration;
            _currentInput.y = input.y * jetpackAcceleration;
        }

        void TryDig()
        {
            if(!canDig) return;

            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numHits = Physics2D.Raycast(transform.position, mousePosition - transform.position, digContactFilter, hits, digRange); 

            if(numHits == 0) return;


            Vector2 normal = hits[0].normal * .25f;


            if(Input.GetMouseButtonDown(0))
            {
                //todo: dont be stupoid
                Tilemap tilemap = hits[0].collider.GetComponent<Tilemap>();
                
                Vector3Int hitPos = tilemap.WorldToCell(hits[0].point);
                GridManager.Instance.MineCell(hitPos);
            }
        }

        private void Move()
        {
            if(!isGrounded && !Input.GetKey(KeyCode.W))
                velocity.y -= 9.8f * Time.deltaTime;
            else if(velocity.y < 0)
                velocity.y = 0;
            
            if(!isGrounded )
            {
                velocity.x -= velocity.x * dragCoefficient * Time.deltaTime;
            }

            if(isGrounded)
            {
                velocity.x -= velocity.x * dragCoefficient * Time.deltaTime * 10;
            }

            float newHorizontalVelocity = Mathf.Clamp(velocity.x + _currentInput.x, -maxMoveSpeed, maxMoveSpeed);
            float newVerticalVelocity = Mathf.Clamp(velocity.y + _currentInput.y, -maxFallSpeed, maxJetpackSpeed);

            velocity = new Vector3 (newHorizontalVelocity, newVerticalVelocity, 0f);

            Debug.Log(_currentInput);
            Debug.Log(velocity);

            _rb.velocity = velocity;

            //Flip the sprite based on velocity
            if(velocity.x < -.2f) 
                _spriteRenderer.flipX = true;
            else if(velocity.x > .2f)
                _spriteRenderer.flipX = false;
        }

    }
}

