using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class CurrencyUpdatedEvent { }
    public class PlayerDiedEvent { }
    public class PlayerRevivedEvent { }
    public class PlayerHpUpdatedEvent : IEvent
    {
        public float PercentHp;

        public PlayerHpUpdatedEvent(float percentHp)
        {
            PercentHp = percentHp;
        }
    }
    public class EnemyKilledEvent { }

    public class UpgradePurchasedEvent : IEvent
    {
        public Upgrade Upgrade;

        public UpgradePurchasedEvent(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }
    
    public class OnHitEffectUnlockedEvent { }

    public class OnCanInteractEvent : IEvent
    {
        public InteractionType InteractionType;

        public OnCanInteractEvent(InteractionType interactionType)
        {
            InteractionType = interactionType;
        }
    }
    
    public class PlayerInteractedEvent : IEvent
    {
        public InteractionType InteractionType;

        public PlayerInteractedEvent(InteractionType interactionType)
        {
            InteractionType = interactionType;
        }
    }

    public class CurrencyRewardEvent: IEvent
    {
        public float MinutesAway;
        public float Award;

        public CurrencyRewardEvent(float minutesAway, float award)
        {
            MinutesAway = minutesAway;
            Award = award;
        }
    }
    
    public class EffectSelectedEvent : IEvent
    {
        public Effect Effect;

        public EffectSelectedEvent(Effect effect)
        {
            Effect = effect;
        }
    }

    public class PurchaseCountChangedEvent : IEvent
    {
        public int PurchaseCount;

        public PurchaseCountChangedEvent(int purchaseCount)
        {
            PurchaseCount = purchaseCount;
        }
    }


    public class PlayerAmmoUpdatedEvent : IEvent
    {
        public int CurrentAmmo;
        public int MaxAmmo;

        public PlayerAmmoUpdatedEvent(int currentAmmo, int maxAmmo)
        {
            CurrentAmmo = currentAmmo;
            MaxAmmo = maxAmmo;
        }
    }

    public class CountryCompletedEvent
    {
    }

    public class WorldCompletedEvent
    {
    }
    public class PlayerChangedDirectionEvent : IEvent
    {
        public Direction NewDirection;
        public PlayerChangedDirectionEvent(Direction direction)
        {
            NewDirection = direction;
        }
    }
    public class PlayerChangedWeaponEvent : IEvent
    {
        public PlayerEntity Entity;
        public PlayerChangedWeaponEvent(PlayerEntity entity)
        {
            Entity = entity;
        }
    }
    public class PlayerResourceUpdateEvent : IEvent
    {
        public ResourceType ResourceType;
        public float Number;
        public PlayerResourceUpdateEvent(ResourceType resourceType, float number)
        {
            ResourceType = resourceType;
            Number = number;
        }
    }

    public class SceneIsReadyEvent : IEvent
    {
        public SceneIsReadyEvent()
        {
        }
    }
}
