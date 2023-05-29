using System;

[Serializable]
public class AnimationName
{
    public string Name;

    public bool CanBeCancelled;

    // Stores the currentIndex to prevent name from being overwritten.
    public int CurrentIndex;
}
