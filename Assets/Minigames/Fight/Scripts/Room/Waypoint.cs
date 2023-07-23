using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Color waypointColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = waypointColor;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
