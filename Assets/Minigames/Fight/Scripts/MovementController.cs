using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public virtual void ApplyMoveEffect(float speedRatio){}
    public virtual void RemoveMoveEffect(float speedRatio){}
}
