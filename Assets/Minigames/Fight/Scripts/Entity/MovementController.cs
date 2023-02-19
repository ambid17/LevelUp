using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    protected float CurrentMoveSpeed;

    protected virtual void SetStartingMoveSpeed(float moveSpeed)
    {
        CurrentMoveSpeed = moveSpeed;
    }

    public virtual void ApplyMoveEffect(float speedRatio)
    {
        CurrentMoveSpeed *= speedRatio;
    }

    public virtual void RemoveMoveEffect(float speedRatio)
    {
        CurrentMoveSpeed /= speedRatio;
    }
}
