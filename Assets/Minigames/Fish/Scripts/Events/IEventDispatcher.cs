using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public interface IEventDispatcher
    {
        void Dispatch<T>();
        void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent;
    }
}