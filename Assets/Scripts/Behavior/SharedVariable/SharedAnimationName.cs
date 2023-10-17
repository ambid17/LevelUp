namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAnimationName : SharedVariable<AnimationName>
    {
        public static implicit operator SharedAnimationName(AnimationName value) { return new SharedAnimationName { Value = value }; }
    }
}