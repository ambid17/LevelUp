using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public WeaponController WeaponController;
    public MovementController MovementController;
    public OrderedList<StatusEffectTracker> StatusEffects = new();

    private void Update()
    {
        foreach (var statusEffect in StatusEffects)
        {
            statusEffect.OnTick(Time.deltaTime);
        }
    }
}
