using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : AnimationManager
{

    [SerializeField]
    private AnimationName idleAnimation;
    [SerializeField]
    private AnimationName moveAnimation;
    [SerializeField]
    private AnimationName attackAnimation;
    [SerializeField]
    private AnimationName dieAnimation;

    public void PlayIdleAnim()
    {
        anim.Play(idleAnimation.Name);
    }

    public void PlayMoveAnim()
    {
        anim.Play(moveAnimation.Name);
    }

    public void PlayAttackAnim()
    {
        anim.Play(attackAnimation.Name);
    }

    public void PlayDieAnim()
    {
        anim.Play(dieAnimation.Name);
    }
}
