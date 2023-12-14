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

            float worldHeight = _camera.orthographicSize * 2;

            float worldWidth = worldHeight * aspect;

            Vector2 target = Vector2.Lerp(playerPos, mousePos, lerpFactor);

            if (_isTransitioning)
            {
                target = GameManager.PlayerEntity.transform.position;
            }

            Vector2 newPos = Vector2.MoveTowards(transform.position, target, lerpSpeed * Time.deltaTime);

            if (!_isTransitioning)
            {
                newPos.x = Mathf.Clamp(newPos.x, _bounds.min.x - (worldWidth / 2), _bounds.max.x + (worldWidth / 2));
                newPos.y = Mathf.Clamp(newPos.y, _bounds.min.y - (worldHeight / 2), _bounds.max.y + (worldHeight / 2));
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
            yield return null;
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