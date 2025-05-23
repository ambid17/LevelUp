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
    public class PlayerHpUpdatedEvent { }
    public class EnemyKilledEvent { }

    public class DidCraftUpgradeEvent { }

    public class ClosedCraftingUiEvent { }
    public class BossEnteredEvent { }

    public class UpgradeCraftedEvent : IEvent
    {
        public Upgrade Upgrade;

        public UpgradeCraftedEvent(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }

    public class MouseHoverEvent : IEvent
    {
        public string Message;
        
        public MouseHoverEvent(string message)
        {
            Message = message;
        }
    }

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


    /// <summary>
    /// Used when the player is controlled, lets it know when the controlled action is finished
    /// </summary>
    public class PlayerControlledActionFinishedEvent : IEvent
    {
        public PlayerControlledActionType ActionType;

        public PlayerControlledActionFinishedEvent(PlayerControlledActionType actionType)
        {
            ActionType = actionType;
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
        public WeaponMode NewWeaponMode;
        public PlayerChangedWeaponEvent(WeaponMode weaponMode)
        {
            NewWeaponMode = weaponMode;
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

    public class EntityStatsFileRemappedEvent : IEvent
    {
        public string StatsFileName;

        public EntityStatsFileRemappedEvent(string statsFileName)
        {
            StatsFileName = statsFileName;
        }
    }
}
