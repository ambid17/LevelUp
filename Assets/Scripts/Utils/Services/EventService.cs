using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class EventService : IEventDispatcher, IEventRegistry {
        private readonly ScopedEventRegistry registry = new ScopedEventRegistry();

        public IEventRegistry Permanent => registry.Permanent;
        
        public EventService() {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        public void Deinit()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene _, Scene __) {
            registry.ClearTransient();
        }

        public void Dispatch<T>() {
            registry.Dispatch<T>();
        }

        public void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent {
            registry.Dispatch(eventClass);
        }

        public void Add<T>(Action action) {
            registry.Transient.Add<T>(action);
        }

        public void Add<IEventType>(Action<IEventType> action) where IEventType : IEvent {
            registry.Transient.Add(action);
        }

        public void Remove<T>(Action action) {
            registry.Transient.Remove<T>(action);
        }

        public void Remove<IEventType>(Action<IEventType> action) where IEventType : IEvent {
            registry.Transient.Remove(action);
        }
    }
}
