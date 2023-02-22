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

    public class UpgradePurchasedEvent : IEvent
    {
        public Upgrade Upgrade;

        public UpgradePurchasedEvent(Upgrade upgrade)
        {
            Upgrade = upgrade;
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
    
    public class UpgradeSelectedEvent : IEvent
    {
        public Upgrade Upgrade;

        public UpgradeSelectedEvent(Upgrade upgrade)
        {
            Upgrade = upgrade;
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
    
    public class PurchaseCountChangedEvent { }


    public class PlayerUsedAmmoEvent : IEvent
    {
        public int CurrentAmmo;
        public int MaxAmmo;

        public PlayerUsedAmmoEvent(int currentAmmo, int maxAmmo)
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
}
