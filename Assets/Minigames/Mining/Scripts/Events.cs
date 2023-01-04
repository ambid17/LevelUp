using Utils;

namespace Minigames.Mining
{
    public class OnCanInteractEvent : IEvent
    {
        public ObjectType ObjectType;
        public OnCanInteractEvent(ObjectType objectType)
        {
            ObjectType = objectType;
        }
    }

    public class OnCantInteractEvent : IEvent
    {
        public ObjectType ObjectType;
        public OnCantInteractEvent(ObjectType objectType)
        {
            ObjectType = objectType;
        }
    }
    public class OnCurrencyUpdatedEvent { }
    public class OnPlayerDamageEvent { }
    public class OnFuelUpdatedEvent { }
    public class OnHealthUpdatedEvent { }
}
