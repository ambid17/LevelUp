using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Minigames.Mining
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _currentInput;
        private Camera _camera;
        private SpriteRenderer _spriteRenderer;
        private Vector2 velocity;
        private EventService _eventService;
        private bool canDig = true;
        private bool isGrounded;
        private Collider2D _collider;
        private float timeSinceDied;
        private Rigidbody2D _rb;
        private float deathTime;

        private void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<OnPlayerDamageEvent>(OnTakeDamage);
        }
        public void SetGrounded(bool g)
        {
            isGrounded = g;
        }

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (GameManager.PlayerSettings.isDead)
            {
                DoDeathMovement();
                timeSinceDied += Time.deltaTime;
            }
            else
            {
                GetMovement();
                Move();
                TryDig();
            }

        }

        private void DoDeathMovement()
        {
            _rb.velocity = Vector3.zero;
            _spriteRenderer.color = Color.grey;
            _collider.enabled = false;
            
            transform.position = Vector3.MoveTowards(transform.position, GameManager.PlayerSettings.spawnPoint, deathTime * Time.deltaTime);
            if(transform.position == GameManager.PlayerSettings.spawnPoint)
            {
                GameManager.PlayerSettings.isDead = false;
                _spriteRenderer.color = Color.white;
                _collider.enabled = true;
                GameManager.MiningProgressSettings.HullHealth = GameManager.MiningProgressSettings.MaxHealth;
            }
        }

        void GetMovement()
        {
            if (GameManager.PlayerSettings.isDead) return;
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
            _currentInput.x = input.x * GameManager.PlayerSettings.acceleration;
            if (_currentInput.x != 0) GameManager.MiningProgressSettings.FuelAmount -= GameManager.PlayerSettings.walkFuelUse * Time.deltaTime;
            _currentInput.y = input.y * GameManager.PlayerSettings.jetpackAcceleration;
            if (_currentInput.y > 0) GameManager.MiningProgressSettings.FuelAmount -= GameManager.PlayerSettings.jetpackFuelUse * Time.deltaTime;

            _eventService.Dispatch<OnFuelUpdatedEvent>();

            if (GameManager.MiningProgressSettings.FuelAmount <= 0)    
                KillPlayer();

        }

        void TryDig()
        {
            if (!canDig) return;

            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numHits = Physics2D.Raycast(transform.position, mousePosition - transform.position, GameManager.PlayerSettings.digContactFilter, hits, GameManager.PlayerSettings.digRange);

            if (numHits == 0) return;


            Vector2 normal = hits[0].normal * .25f;


            if (Input.GetMouseButtonDown(0))
            {
                //todo: dont be stupoid

                GameManager.GridService.MineCell(hits[0].point);
            }
        }

        private void Move()
        {
            if (!isGrounded && !Input.GetKey(KeyCode.W))
                velocity.y -= 9.8f * Time.deltaTime;
            else if (velocity.y < 0)
                velocity.y = 0;

            if (!isGrounded)
            {
                velocity.x -= velocity.x * GameManager.PlayerSettings.dragCoefficient * Time.deltaTime;
            }

            if (isGrounded)
            {
                velocity.x -= velocity.x * GameManager.PlayerSettings.dragCoefficient * Time.deltaTime * 10;
            }

            float newHorizontalVelocity = Mathf.Clamp(velocity.x + _currentInput.x, -GameManager.PlayerSettings.maxMoveSpeed, GameManager.PlayerSettings.maxMoveSpeed);
            float newVerticalVelocity = Mathf.Clamp(velocity.y + _currentInput.y, -GameManager.PlayerSettings.maxFallSpeed, GameManager.PlayerSettings.maxJetpackSpeed);

            velocity = new Vector3(newHorizontalVelocity, newVerticalVelocity, 0f);


            _rb.velocity = velocity;

            //Flip the sprite based on velocity
            if (velocity.x < -.2f)
                _spriteRenderer.flipX = true;
            else if (velocity.x > .2f)
                _spriteRenderer.flipX = false;
        }

        void OnTakeDamage()
        {
            if (GameManager.MiningProgressSettings.HullHealth <= 0)
            {
                KillPlayer();
            }
        }

        private void KillPlayer()
        {
            GameManager.PlayerSettings.isDead = true;
            Vector3 distanceFromHome = transform.position - GameManager.PlayerSettings.spawnPoint;
            deathTime = distanceFromHome.magnitude / GameManager.PlayerSettings.deathAnimationLength;
            GameManager.MiningProgressSettings.HullHealth = 0;
            _eventService.Dispatch<OnHealthUpdatedEvent>();
        }
    }
}

