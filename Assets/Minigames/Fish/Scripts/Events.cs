using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace Minigames.Fish
{
    public class ResetGameEvent{ }
    public class ReeledInEvent { }

    public class FishCaughtEvent : IEvent
    {
        public FishInstanceSettings Fish;

        public FishCaughtEvent(FishInstanceSettings fish)
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

    public class CurrencyUpdatedEvent { }

    public class FishOnLureUpdatedEvent { }

    public class LureNextDepthEvent { }

    public class LureSnappedEvent { }
}
