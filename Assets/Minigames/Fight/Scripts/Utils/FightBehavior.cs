using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class FightBehavior : MonoBehaviour
    {
        protected EventService eventService;

        protected virtual void Awake()
        {
            eventService = GameManager.EventService;
        }
    }
}