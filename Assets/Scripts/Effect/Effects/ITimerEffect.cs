using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimerEffect
{
    public float TickRate { get; }
    public void OnTick(Entity source, List<Entity> targets);
    public List<Entity> GetTargets();
}
