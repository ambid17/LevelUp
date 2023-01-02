using System.Collections;
using System.Collections.Generic;
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
}
