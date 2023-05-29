using System.Collections;
using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    public bool IsAnimFinished => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected float maxBufferPercent = .8f;

    private AnimationName bufferedAnimation;

    private AnimationName currentAnimation = new();

    protected bool IsAnimPlaying(AnimationName name)
    {
        return name == currentAnimation;
    }
    public void PlayAnimation(AnimationName name)
    {
        if (IsAnimPlaying(name))
        {
            return;
        }
        if (!currentAnimation.CanBeCancelled)
        {
            QueAnimation(name);
            return;
        }
        anim.Play(name.Name);
        currentAnimation = name;
    }
    public void QueAnimation(AnimationName name)
    {
        if (bufferedAnimation != null)
        {
            return;
        }
        if (IsAnimPlaying(name))
        {
            return;
        }
        if (!IsAnimFinished)
        {
            float remainingTime = Mathf.Ceil(anim.GetCurrentAnimatorStateInfo(0).normalizedTime) - anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (remainingTime >= maxBufferPercent)
            {
                return;
            }
            StartCoroutine(PlayQuedAnimation(name, remainingTime));
        }
    }
    private IEnumerator PlayQuedAnimation(AnimationName name, float remainingTime)
    {
        yield return new WaitForSeconds(remainingTime);
        bufferedAnimation = null;
        PlayAnimation(name);
    }
}
