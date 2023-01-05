using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public interface IEventDispatcher
    {
        void Dispatch<T>();
        void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent;
    }
}