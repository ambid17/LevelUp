using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class CameraLerp : MonoBehaviour
    {
        public Bounds CameraBounds { get => _bounds; set => _bounds = value; }

        [SerializeField]
        private float lerpFactor;
        [SerializeField]
        private float lerpSpeed = 5f;
        [SerializeField]
        private float zoomOrthagraphicSize;
        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float startingOrthographicSize;
        [SerializeField]
        private float transitionTime;
        [SerializeField]
        private float deadRadius;
        [SerializeField]
        private float transitionLerpMultiplier;

        private Bounds _bounds = new();
        private Camera _camera;
        private bool _isTransitioning;


        private void Start()
        {
            _camera = GetComponent<Camera>();
            _camera.orthographicSize = startingOrthographicSize;
        }

        private void Update()
        {
            Vector2 mousePos = GameManager.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = GameManager.PlayerEntity.transform.position;

            float aspect = (float)Screen.width / Screen.height;

            float cameraViewHeight = _camera.orthographicSize * 2;

            float cameraViewWidth = cameraViewHeight * aspect;

            Vector2 target = Vector2.zero;

            // If transitioning, the target is just getting the camera into the bounds of the new room
            if (_isTransitioning)
            {
                target.x = Mathf.Clamp(target.x, _bounds.min.x + (cameraViewWidth / 2), _bounds.max.x - (cameraViewWidth / 2));
                target.y = Mathf.Clamp(target.y, _bounds.min.y + (cameraViewHeight / 2), _bounds.max.y - (cameraViewHeight / 2));
            }
            else
            {
                target = Vector2.Lerp(playerPos, mousePos, lerpFactor);
            }

            // while the camera is transitioning, it needs to move fast enough to be done, otherwise the camera will snap to in bounds when the transition finishes
            float calculatedLerpSpeed = _isTransitioning ? lerpSpeed * transitionLerpMultiplier : lerpSpeed;
            Vector2 newPos = Vector2.MoveTowards(transform.position, target, calculatedLerpSpeed * Time.deltaTime);

            if (!_isTransitioning)
            {
                newPos.x = Mathf.Clamp(newPos.x, _bounds.min.x + (cameraViewWidth / 2), _bounds.max.x - (cameraViewWidth / 2));
                newPos.y = Mathf.Clamp(newPos.y, _bounds.min.y + (cameraViewHeight / 2), _bounds.max.y - (cameraViewHeight / 2));
            }

            transform.position = new Vector3(newPos.x, newPos.y, -10);
        }

        public void Transition(Bounds newBounds)
        {
            _bounds = newBounds;

            // Stop the coroutine before restarting it in case player switches rooms twice before the first one is done.
            StopCoroutine(MoveToNextRoom());
            StartCoroutine(MoveToNextRoom());
        }

        IEnumerator MoveToNextRoom()
        {
            _isTransitioning = true;
            while (_camera.orthographicSize > zoomOrthagraphicSize)
            {
                _camera.orthographicSize -= zoomSpeed * Time.deltaTime;
                yield return new WaitForSeconds(0);
            }
            _camera.orthographicSize = zoomOrthagraphicSize;
            yield return new WaitForSeconds(transitionTime);
            while (_camera.orthographicSize < startingOrthographicSize)
            {
                _camera.orthographicSize += zoomSpeed * Time.deltaTime;
                yield return new WaitForSeconds(0);
            }
            _camera.orthographicSize = startingOrthographicSize;
            _isTransitioning = false;
        }

    }
}