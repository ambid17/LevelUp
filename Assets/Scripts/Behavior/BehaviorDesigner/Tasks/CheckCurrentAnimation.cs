namespace BehaviorDesigner.Runtime.Tasks
{

    [TaskDescription("Checks if the selected animation is the one currently being played.")]
    [TaskCategory("Movement/Animation")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}AnimationIcon.png")]
    public class CheckCurrentAnimation : CustomAnimator
    {
        [Tooltip("The animation we're checking for")]
        public SharedAnimationName Animation;

        public override TaskStatus OnUpdate()
        {
            if (animationManager.CurrentAnimation == Animation.Value)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}