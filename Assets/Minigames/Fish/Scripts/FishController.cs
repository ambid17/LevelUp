using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fish
{
    public class FishController : MonoBehaviour
    {
        private FishInstanceSettings _fish;
        public FishInstanceSettings Fish => _fish;

        private Vector2 _currentTarget;
        private Bounds _targetBounds;
        void Start()
        {
        
        }

        void Update()
        {
            if (_currentTarget == Vector2.zero)
            {
                GetNextTarget();
            }

            Vector2 currentPosition = transform.position;
            Vector2 newPos = Vector2.MoveTowards(currentPosition, _currentTarget, _fish.moveSpeed * Time.deltaTime);
            transform.position = newPos;
        }

        private void GetNextTarget()
        {
            _currentTarget = PhysicsUtils.RandomPointInBounds(_targetBounds);
        }

        public void Setup(FishInstanceSettings instanceSettings, Bounds targetBounds)
        {
            _fish = instanceSettings;
            _targetBounds = targetBounds;
        }
    }
}

