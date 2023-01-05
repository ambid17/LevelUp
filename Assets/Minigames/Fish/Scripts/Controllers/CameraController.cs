using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace Minigames.Fish
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _smoothTime;
        private Vector3 _cameraVelocity;
        private Lure _currentLure;
        private EventService _eventService;
        private Vector3 _targetPosition;
        private Vector3 _defaultPosition;
        
        void Start()
        {
            _defaultPosition = transform.position;
            
            _eventService = Services.Instance.EventService;
            _eventService.Add<WaitingForSlingshotEvent>(ResetCamera);
            _eventService.Add<LureSnappedEvent>(ResetCamera);
            _eventService.Add<LureThrownEvent>(TargetLure);
        }

        void Update()
        {
            if (_currentLure != null) {
                _targetPosition = _currentLure.transform.position;
                _targetPosition.z = _defaultPosition.z;
            }

            var dampPosition = Vector3.SmoothDamp(transform.position, _targetPosition, ref _cameraVelocity, _smoothTime);
            transform.position = dampPosition;
        }
        
        void TargetLure(LureThrownEvent eventType) {
            _currentLure = eventType.Lure;
        }
        
        void ResetCamera() {
            _currentLure = null;
            _targetPosition = _defaultPosition;
        }
    }
}

