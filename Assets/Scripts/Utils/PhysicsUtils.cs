using UnityEngine;

public static class PhysicsUtils
{
    public static readonly int PlayerLayer = 6;
    public static int ProjectileLayer = 7;
    public static int GroundLayer = 8;
    public static int EnemyLayer = 9;

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
    public static Quaternion LookAt(Quaternion rotation, Vector3 myPosition, Vector3 targetPosition, float lerpFactor = 1)
    {
        Vector3 direction = (targetPosition - myPosition);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion finalRotation = Quaternion.Euler(new Vector3(0,0,angle));
        Vector3 angles = finalRotation.eulerAngles;
        //var toReturn = Quaternion.Slerp(rotation, finalRotation, lerpFactor);
        var toReturn = finalRotation;
        Vector3 toReturnAngles = toReturn.eulerAngles;
        return toReturn;
    }
}