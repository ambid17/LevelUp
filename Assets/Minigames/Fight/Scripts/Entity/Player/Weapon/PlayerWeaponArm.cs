using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponArm : MonoBehaviour
{
    public SpriteRenderer MySpriteRenderer;
    public float StartRotation;
    public float MinRotation;
    public float MaxRotation;

    [SerializeField]
    private float returnToIdleSpeed = 2;

    public void ReturnToIdle()
    {
        StartCoroutine(RotateTowardsZero());
    }
    IEnumerator RotateTowardsZero()
    {
        while (transform.eulerAngles.z != 0)
        {
            transform.rotation = PhysicsUtils.LookAt(transform, transform.position, 0, returnToIdleSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
