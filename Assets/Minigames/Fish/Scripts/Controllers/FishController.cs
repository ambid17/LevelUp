using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Minigames.Fight;
using UnityEngine;
using Utils;

namespace Minigames.Fish
{
    public class FishController : MonoBehaviour
    {
        private BoxCollider2D _collider;
        private EventService _eventService;
        
        private FishInstanceSettings _fish;
        public FishInstanceSettings Fish => _fish;

        private Vector2 _currentTarget;
        private Bounds _targetBounds;
        private bool _isCaught;
        
        void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _eventService = Services.Instance.EventService;
        }

        void Update()
        {
            if (_isCaught) return;
            
            if (_currentTarget == Vector2.zero || transform.position.AsVector2() == _currentTarget)
            {
                GetNextTarget();
            }

            Vector2 currentPosition = transform.position;
            Vector2 newPos = Vector2.MoveTowards(currentPosition, _currentTarget, _fish.MoveSpeed * Time.deltaTime);
            transform.position = newPos;
        }

        private void GetNextTarget()
        {
            _currentTarget = _targetBounds.RandomPointInBounds();
        }

        public void Setup(FishInstanceSettings instanceSettings, Bounds targetBounds)
        {
            _fish = instanceSettings;
            _targetBounds = targetBounds;
        }

        public void Catch(Transform transformToFollow)
        {
            _isCaught = true;
            _collider.enabled = false;
            transform.parent = transformToFollow;
        }
    }
}

