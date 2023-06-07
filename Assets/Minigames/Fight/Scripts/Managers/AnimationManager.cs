using System.Collections;
using UnityEngine;

public abstract class AnimationManager : MonoBehaviour
{
    public bool IsAnimFinished => CurrentAnimationNomralizedTime >= 1;
    public float CurrentAnimationNomralizedTime => anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

    [SerializeField]
    protected Animator anim;

    private AnimationName bufferedAnimation;

    private AnimationName currentAnimation = new();

    protected bool IsAnimPlaying(AnimationName name)
    {
        return name == currentAnimation;
    }

    // Returns true if the normalized difference between current normalized time and next loop is less than acceptableDifference.
    protected bool IsCurrentAnimLoopFinished(float acceptableDifference)
    {
        return (Mathf.Ceil(CurrentAnimationNomralizedTime) - CurrentAnimationNomralizedTime) <= acceptableDifference;
    }
    public void PlayAnimation(AnimationName name, float time)
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
        anim.Play(name.Name, 0, time);
        currentAnimation = name;
    }
    public void OverrideAnimation(AnimationName name, float normalizedTime)
    {
        if (IsAnimPlaying(name))
        {
            return;
        }
        anim.Play(name.Name, 0, normalizedTime);
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
        if (!IsCurrentAnimLoopFinished(name.AcceptableOverrideTime))
        {
            float remainingTime = Mathf.Ceil(CurrentAnimationNomralizedTime) - CurrentAnimationNomralizedTime;
            if (remainingTime >= name.MaxBufferPercentage)
            {
                return;
            }
            StartCoroutine(PlayQuedAnimation(name));
        }
        OverrideAnimation(name, 0);
    }
    private IEnumerator PlayQuedAnimation(AnimationName name)
    {
        bufferedAnimation = name;
        while (!IsCurrentAnimLoopFinished(name.AcceptableOverrideTime))
        {
            yield return null;
        }
        OverrideAnimation(name, 0);
        bufferedAnimation = null;
        PlayAnimation(name, 0);
    }
}
