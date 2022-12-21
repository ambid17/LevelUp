using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Mining
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Vector2 _currentInput;
        private Camera _camera;
        private SpriteRenderer _spriteRenderer;
        
        private bool canDig = true;
        [SerializeField] private float digRange;
        [SerializeField] private ContactFilter2D digContactFilter;
        [SerializeField] private float moveSpeed = 5;

        void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
                Debug.Log("ASd");
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

            _currentInput = input * moveSpeed;
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
                Tilemap tilemap = hits[0].collider.GetComponent<Tilemap>();
                
                Vector3Int hitPos = tilemap.WorldToCell(hits[0].point);
                tilemap.SetTile(hitPos, null);
            }
        }

        private void Move()
        {
            _rigidbody2D.AddForce(_currentInput);


            //Flip the sprite based on velocity
            if(_rigidbody2D.velocity.x < 0) 
                _spriteRenderer.flipX = true;
            else 
                _spriteRenderer.flipX = false;
        }
    }
}

