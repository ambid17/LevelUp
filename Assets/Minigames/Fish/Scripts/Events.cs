using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class ResetGameEvent{ }
    public class EndGameEvent { }
    public class ProjectileExplodedEvent { }
    public class ProjectileCollidedEvent { }

    public class FishCaughtEvent : IEvent
    {
        public Fish Fish;

        public FishCaughtEvent(Fish fish)
        {
            Fish = fish;
        }
    }
    public class SlingReadyWithProjectile { }
    public class BeginFlingEvent { }
    public class ToggleMusicEvent { }

    public class LureThrownEvent : IEvent
    {
        public Lure Lure;

        public LureThrownEvent(Lure lure)
        {
            Lure = lure;
        }
    }
    
    public class EntitySelectEvent : IEvent
    {
        public int EntityId;

        public EntitySelectEvent(int entityId)
        {
            EntityId = entityId;
        }
    }
    
    public class PropSizeEvent : IEvent
    {
        public Vector2 NewPropSize;

        public PropSizeEvent(Vector2 newPropSize)
        {
            NewPropSize = newPropSize;
        }
    }

    public class LevelLoadedEvent { }

    public class PowerupAcquiredEvent : IEvent
    {
        public Vector3 position;
        public int powerupType;

        public PowerupAcquiredEvent(Vector3 position, int powerupType)
        {
            this.position = position;
            this.powerupType = powerupType;
        }
    }
}
