using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fish
{
    public class Lure : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;

        private EventService _eventService;

        private bool _isThrown;
        private Projectile _projectile;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }

        private void Start() {
            _eventService = Services.Instance.EventService;
            _eventService.Add<LureThrownEvent>(OnThrown);
        }
        
        private void OnThrown(LureThrownEvent thrownEvent) {
            _eventService.Remove<LureThrownEvent>(OnThrown);
        }

        private void Update() {
            if (_isThrown && _rigidbody.IsSleeping()) {
                Destroy(gameObject);
            }
        }

        public void Setup(Projectile projectile)
        {
            _projectile = projectile;
            _spriteRenderer.sprite = projectile.Sprite;
        }
        
        public void Throw(Vector3 velocity) {
            _isThrown = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // if its a fish add it to our list of fishies
        }

        private void OnDestroy()
        {
            // TODO unsubscribe from events
        }
    }
}
