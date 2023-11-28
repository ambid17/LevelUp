using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimationPlayer : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    [SerializeField]
    private Animator Anim;
    [SerializeField]
    private AnimationName DeathAnim;

    public void Start()
    {
        Anim.Play(DeathAnim.Name);
    }
}
