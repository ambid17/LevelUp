using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class ResetGameEvent{ }
    public class ReeledInEvent { }

    public class FishCaughtEvent : IEvent
    {
        public Fish Fish;

        public FishCaughtEvent(Fish fish)
        {
            Fish = fish;
        }
    }
    public class WaitingForSlingshotEvent { }

    public class LureThrownEvent : IEvent
    {
        public Lure Lure;

        public LureThrownEvent(Lure lure)
        {
            Lure = lure;
        }
    }
}
