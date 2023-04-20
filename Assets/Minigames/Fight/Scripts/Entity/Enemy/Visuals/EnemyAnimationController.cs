using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField]
    private AnimationName animationName;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play(animationName.Name);
    }
}
