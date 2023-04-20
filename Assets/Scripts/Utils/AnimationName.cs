using System;
using UnityEngine;

[Serializable]
public class AnimationName
{
    public string Name;

    // Stores the currentIndex to prevent name from being overwritten.
    public int CurrentIndex;
}
