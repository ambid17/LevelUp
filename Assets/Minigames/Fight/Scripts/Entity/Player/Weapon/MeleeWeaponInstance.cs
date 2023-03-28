using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponInstance : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public bool IsAnimFinished => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    public bool GetCurrentAnim(string animName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (IsAnimFinished)
        {
            TriggerAnimation("idle");
        }
    }

    public void TriggerAnimation(string anim)
    {
        if (!GetCurrentAnim(anim) && IsAnimFinished)
        {
            _animator.Play(anim);
        }
    }
}
