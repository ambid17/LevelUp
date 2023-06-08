using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class CurrencyUpdatedEvent { }
    public class CpmUpdatedEvent { }
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

    public class EffectPurchasedEvent : IEvent
    {
        public Effect Effect;

        public EffectPurchasedEvent(Effect effect)
        {
            Effect = effect;
        }
    }
    
    public class OnHitEffectUnlockedEvent { }

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
    
    public class EffectItemSelectedEvent : IEvent
    {
        public EffectItem EffectItem;

        public EffectItemSelectedEvent(EffectItem effectItem)
        {
            EffectItem = effectItem;
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
    
    public class PlayerUsedAbilityEvent
    {
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
        public WeaponMode WeaponMode;
        public PlayerChangedWeaponEvent(WeaponMode weaponMode)
        {
            WeaponMode = weaponMode;
        }
    }
}
