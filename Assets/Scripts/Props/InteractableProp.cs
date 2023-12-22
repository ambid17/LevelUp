using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// This exists in the start room.
    /// You spent DNA on:
    /// - meta upgrades
    /// - unlock new effects
    /// - upgrade equipped effects
    ///
    /// It also allows you to bank DNA
    /// </summary>
    ///
    /// /// <summary>
    /// This exists in the boss room.
    /// You spent Physical Resources on:
    /// - being able to equip an effect
    /// </summary>
    public class InteractableProp : MonoBehaviour
    {
        [SerializeField] private InteractionType InteractionType;


        private void Start()
        {
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                Platform.EventService.Dispatch(new OnCanInteractEvent(InteractionType));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                Platform.EventService.Dispatch(new OnCanInteractEvent(InteractionType.None));
            }
        }
    }
}