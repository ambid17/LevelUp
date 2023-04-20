using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    protected bool IsAnimFinished => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

    [SerializeField]
    protected Animator anim;

    protected bool IsAnimPlaying(AnimationName name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name.Name);
    }
}
