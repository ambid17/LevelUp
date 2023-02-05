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
    public class PlayerDamagedEvent { }
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
}
