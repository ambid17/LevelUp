using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Transform _rotator;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _timePerSegment;
    [SerializeField] private int _lineSegments;
    
    public Vector3 FirePosition => _rotator.position;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        ClearTrajectory();
    }
    
    public void UpdateRotation(Quaternion rotation) {
        _rotator.rotation = rotation;
    }

    public void UpdateTrajectory(Vector2 startPos, Vector2 startVelocity)
    {
        ClearTrajectory();
        _lineRenderer.positionCount = _lineSegments;
        for (int i = 0; i < _lineSegments; i++)
        {
            Vector3 position = GetPositionAtTime(startPos, startVelocity, _timePerSegment * i);
            _lineRenderer.SetPosition(i, position);
        }
    }

    public void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }

    // Equation for calculating position of projectile:
    // position = vector2(startX + xVel * time, startY + yVel * time - gravity * time^2 / 2)
    private Vector2 GetPositionAtTime(Vector2 startPos, Vector2 startVelocity, float time)
    {
        float xPos = startPos.x + (startVelocity.x * time);

        float gravityAtTime = Physics2D.gravity.y / 2 * Mathf.Pow(time, 2);
        // we are adding gravity instead of subtracting since the y velocity is negative
        float yPos = startPos.y + (startVelocity.y * time) + gravityAtTime;
        Vector2 positionAtTime = new Vector3(xPos, yPos);
        return positionAtTime;
    }
}
