using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Transform rotator;
    
    public Vector3 FirePosition => rotator.position;
    
    
    public void UpdateRotation(Quaternion rotation, float fill) {
        rotator.rotation = rotation;
    }
}
