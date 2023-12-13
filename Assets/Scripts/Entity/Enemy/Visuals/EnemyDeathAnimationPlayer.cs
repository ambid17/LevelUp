using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyDeathAnimationPlayer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator anim;

    public void Setup(SpriteRenderer renderer, AnimatorController animation)
    {
        spriteRenderer.sprite = renderer.sprite;
        spriteRenderer.flipX = renderer.flipX;
        anim.runtimeAnimatorController = animation;
    }
}
