using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public bool IsAnimFinished => CurrentAnimationNomralizedTime >= 1;
    public float CurrentAnimationNomralizedTime => anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    public AnimationName CurrentAnimation => currentAnimation;

    [SerializeField]
    protected Animator anim;

    private AnimationName bufferedAnimation;

    protected AnimationName currentAnimation;

    protected bool IsAnimPlaying(AnimationName name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name.Name);
    }

    // Returns true if the normalized difference between current normalized time and next loop is less than acceptableDifference.
    public bool IsCurrentAnimLoopFinished(float acceptableDifference)
    {
        float difference = Mathf.Ceil(CurrentAnimationNomralizedTime) - CurrentAnimationNomralizedTime;
        return  Mathf.Clamp(difference, 1 - acceptableDifference, 1 + acceptableDifference) == difference;
    }

    public void ResetAnimations()
    {
        currentAnimation = null;
    }

    public void PlayAnimation(AnimationName name, float time)
    {
        // Prevent null refs
        if (currentAnimation == null)
        {
            currentAnimation = name;
        }

        // Don't allow queueing up the same animation
        if (IsAnimPlaying(name))
        {
            return;
        }

        if (!currentAnimation.CanBeCancelled)
        {
            QueueAnimation(name);
            return;
        }

        anim.Play(name.Name, 0, time);
        currentAnimation = name;
    }

    // Force play an animation even if current can't be canceled.
    public void OverrideAnimation(AnimationName name, float normalizedTime)
    {
        // Don't allow playing the same animation
        if (IsAnimPlaying(name))
        {
            return;
        }
        anim.Play(name.Name, 0, normalizedTime);
        currentAnimation = name;
    }

    public void QueueAnimation(AnimationName name)
    {
        if (bufferedAnimation != null || IsAnimPlaying(name))
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
            return;
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
    }
}
