using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Services
    {
        private static Services instance;
        public static Services Instance => instance ??= new Services();

        private EventService _eventService;
        public EventService EventService
        {
            get
            {
                if (_eventService == null)
                {
                    _eventService = new EventService();
                }

                return _eventService;
            }
        }

        public static void ClearInstance()
        {
            instance.EventService.Deinit();
            instance = null;
        }
    }
}