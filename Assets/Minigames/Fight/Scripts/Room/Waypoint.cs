using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType
{
    Spawn, Patrol, Worker, Flower
}

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Color waypointColor;

    public WaypointType waypointType;
    private void OnDrawGizmos()
    {
        Gizmos.color = waypointColor;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
