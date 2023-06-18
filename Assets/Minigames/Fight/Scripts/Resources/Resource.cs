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

        private float _myResourceValue = 1;

        public void Setup(Sprite sprite, ResourceType resourceType)
        {
            mySpriteRenderer.sprite = sprite;
            myResourceType = resourceType;

            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            float speed = Random.Range(minSpeed, maxSpeed);
            myRigidbody.AddForce(new Vector2(x, y) * speed, ForceMode2D.Impulse);

            GameManager.EventService.Add<PlayerDiedEvent>(Die);
        }

        public void Die()
        {
            Destroy(gameObject);
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
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_hasStopped)
            {
                return;
            }
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                GameManager.CurrencyManager.AddResource(myResourceType, _myResourceValue);
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            GameManager.EventService.Remove<PlayerDiedEvent>(Die);
        }
    }
}