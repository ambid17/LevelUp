using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtils
{
    public static readonly int PlayerLayer = 6;
    public static int ProjectileLayer = 7;
    public static int ObstacleLayer = 10;
    public static int wallLayer = 12;
    public static int EnemyLayer = 9;

    public static int groundGraph = 1;
    public static int flyGraph = 2;

    public static Vector2 AsVector2(this Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }

    public static Vector2 GetRandomInDonut(float minDistance, float maxDistance)
    {
        Vector2 point = Random.insideUnitCircle;
        point = point.normalized;
        point *= Random.Range(minDistance, maxDistance);

        return point;
    }

    public static Vector2 RandomPointInBounds(this Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    public static Vector2 Rotate(this Vector2 point, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float x = point.x;
        float y = point.y;

        return new Vector2((cos * x) - (sin * y), (sin * x) + (cos * y));
    }

    public static Vector3 Clamp(this Vector3 toClamp, Vector3 min, Vector3 max)
    {
        Vector3 toReturn = toClamp;
        toReturn.x = Mathf.Clamp(toReturn.x, min.x, max.x);
        toReturn.y = Mathf.Clamp(toReturn.y, min.y, max.y);
        toReturn.z = Mathf.Clamp(toReturn.z, min.z, max.z);
        return toReturn;
    }
    public static Quaternion LookAt(Transform transform, Vector3 targetPosition, float startAngle, float lerpFactor = 1)
    {
        Vector3 direction = (targetPosition - transform.position);
        // have to subtract 90 degrees.
        // This assumes that 0 degrees pointing right, in unity it is pointing up
        // Had to change this to subtract a variable instead because the angle of a sprite
        // at 0 degrees is dependant on the art.
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - startAngle;
        Quaternion finalRotation = Quaternion.Euler(new Vector3(0,0,angle));
        return Quaternion.Slerp(transform.rotation, finalRotation, lerpFactor);
    }

    /// <summary>
    /// To determine if an object is in an arc of another it has to fit 2 criteria:
    /// - the distance between the objects has to be less than the radius of the cone
    /// - the angle between the center-line of the cone and the object must be less than the angle of the cone
    /// </summary>
    /// <returns></returns>
    public static GameObject HasLineOfSight(Transform source, Transform target, float distance, float fieldOfView, LayerMask layerMask)
    {
        Vector2 toTarget = target.position.AsVector2() - source.position.AsVector2();
        // Check if target is in range
        if (toTarget.magnitude > distance)
        {
            return null;
        }

        RaycastHit2D hit = Physics2D.Linecast(source.position, target.position, layerMask);

        // Check we have line of sight of the correct target
        if(hit.collider.gameObject.GetInstanceID() != target.gameObject.GetInstanceID())
        {
            return null;
        }
        
        // Check line of sight is in FOV
        Vector3 forward = source.up; // in 2d, the y-axis is our rotation
        float dot = Vector2.Dot(toTarget, forward);
        float totalMagnitude = toTarget.magnitude * forward.magnitude;
        float angleToTarget = Mathf.Acos(dot / totalMagnitude) * Mathf.Rad2Deg;
        float maxAngle = fieldOfView / 2;

        if (angleToTarget < maxAngle)
        {
            return hit.collider.gameObject;
        }

        return null;

    }
    public static Vector2 AsVector2(this BoundsInt boundsInt)
    {
        return new Vector2(boundsInt.xMax - boundsInt.xMin, boundsInt.yMax - boundsInt.yMin);
    }
    public static Vector2 AsVector2(this Vector3Int vector3Int)
    {
        return new Vector2(vector3Int.x, vector3Int.y);
    }
    public static Vector2 RandomAroundTarget(Vector2 target, float randomFactor)
    {
        Vector2 randomOffset = new Vector2(Random.Range(-randomFactor, randomFactor), Random.Range(-randomFactor, randomFactor));
        Vector2 newPosition = target + randomOffset;
        return newPosition;
    }
}