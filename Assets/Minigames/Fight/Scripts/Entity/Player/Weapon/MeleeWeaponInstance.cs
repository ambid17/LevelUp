using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponInstance : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    void Start()
    {
        
    }

    public void TriggerAnimation(string anim)
    {
        _animator.SetTrigger(anim);
    }
}
