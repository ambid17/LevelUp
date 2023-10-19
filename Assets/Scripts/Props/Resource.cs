using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class Resource : MonoBehaviour
    {

        [SerializeField]
        private float attractDistance = 10f;
        [SerializeField]
        private float attractSpeed = 20f;
        [SerializeField]
        private SpriteRenderer mySpriteRenderer;
        [SerializeField]
        private ResourceType myResourceType;
        [SerializeField]
        private Rigidbody2D myRigidbody;
        [SerializeField]
        private float minSpeed = 5f;
        [SerializeField]
        private float maxSpeed = 10f;
        [SerializeField]
        private float deceleration = 0.5f;

        private bool _hasStopped;

        private bool _isColliding;

        private bool _isMarkedForDeath;

        private float _resourceValue;

        public void Setup(Sprite sprite, ResourceType resourceType, float resourceValue)
        {
            mySpriteRenderer.sprite = sprite;
            myResourceType = resourceType;
            _resourceValue = resourceValue;

            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float speed = Random.Range(minSpeed, maxSpeed);
            myRigidbody.AddForce(new Vector2(x, y).normalized * speed, ForceMode2D.Impulse);

            Platform.EventService.Add<PlayerDiedEvent>(Die);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private void TryDie()
        {
            if (_isMarkedForDeath)
            {
                return;
            }
            _isMarkedForDeath = true;
            GameManager.CurrencyManager.AddResource(myResourceType, _resourceValue);
            Die();
        }

        private void Update()
        {
            if (!_hasStopped)
            {
                if (myRigidbody.velocity.magnitude > 0.1f)
                {
                    myRigidbody.velocity -= myRigidbody.velocity * deceleration * Time.deltaTime;
                    return;
                }
                _hasStopped = true;
            }
            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            if (Vector2.Distance(transform.position, GameManager.PlayerEntity.transform.position) < attractDistance)
            {
                Vector2 direction = GameManager.PlayerEntity.transform.position - transform.position;
                myRigidbody.velocity = direction.normalized * attractSpeed;
            }
            if (_isColliding)
            {
                TryDie();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                _isColliding = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                _isColliding = false;
            }
        }
        private void OnDestroy()
        {
            Platform.EventService.Remove<PlayerDiedEvent>(Die);
        }
    }
}