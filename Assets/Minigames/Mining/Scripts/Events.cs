using Utils;

namespace Minigames.Mining
{
    public class OnCanInteractEvent
    {
        public ObjectType ObjectType;
        public OnCanInteractEvent(ObjectType objectType)
        {
            ObjectType = objectType;
        }
    }

}
