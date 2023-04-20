using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    public bool IsAnimFinished => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

    [SerializeField]
    protected Animator anim;

    protected bool IsAnimPlaying(AnimationName name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name.Name);
    }
    public void PlayAnimation(AnimationName name)
    {
        if (IsAnimPlaying(name))
        {
            return;
        }
        anim.Play(name.Name);
    }
}
