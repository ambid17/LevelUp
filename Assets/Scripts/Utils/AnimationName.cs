using System;

[Serializable]
public class AnimationName
{
    public string Name;

    public bool CanBeCancelled;

    public float MaxBufferPercentage = .8f;

    public float AcceptableOverrideTime = .05f;

    // Stores the currentIndex to prevent name from being overwritten.
    public int CurrentIndex;
}
