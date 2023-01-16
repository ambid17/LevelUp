using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _zoomSensitivity;
    [SerializeField] private float _closestZ;
    [SerializeField] private float _farthestZ;

    private Camera _camera;

    private bool _isDragging;
    private Vector3 _dragStartPosition;
    
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _dragStartPosition = GetWorldPosition();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector3 currentMousePos = GetWorldPosition();
            Vector3 offset = _dragStartPosition - currentMousePos;
            transform.position += offset;
        }

        if (Input.mouseScrollDelta.magnitude != 0)
        {
            Vector3 newPos = transform.position;

            float newZ = Input.mouseScrollDelta.y * _zoomSensitivity * Time.deltaTime;
            newPos.z += newZ;
            newPos.z = Mathf.Clamp(newPos.z, _farthestZ, _closestZ);
            transform.position = newPos;
        }
    }


    private Vector3 GetWorldPosition()
    {
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        ground.Raycast(mousePos, out float distance);
        return mousePos.GetPoint(distance);
    }
}
