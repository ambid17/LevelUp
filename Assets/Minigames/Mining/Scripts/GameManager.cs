using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Utils;

namespace Minigames.Mining
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private GridService _gridService;
        [SerializeField]
        private PlayerController _playerController;
        [SerializeField] TileSettings _tileSettings;
        [SerializeField] MiningProgressSettings _miningProgressSettings;
        [SerializeField] ProgressSettings _progressSettings;
        [SerializeField] PlayerSettings _playerSettings;
        
        public static PlayerController PlayerController => Instance._playerController;
        public static GridService GridService => Instance._gridService;
        public static TileSettings TileSettings => Instance._tileSettings;
        public static MiningProgressSettings MiningProgressSettings => Instance._miningProgressSettings;
        public static ProgressSettings ProgressSettings => Instance._progressSettings;
        public static PlayerSettings PlayerSettings => Instance._playerSettings;
        
        private EventService _eventService;
        public static EventService EventService
        {
            get
            {
                if (Instance._eventService == null)
                {
                    Instance._eventService = new EventService();
                }

                return Instance._eventService;
            }
        }
        
        //todo: this but for the rest of them
        public static float Currency
        {
            get => Instance._progressSettings.Currency;
            set
            {
                Instance._progressSettings.Currency = value;
                Instance._eventService.Dispatch<OnCurrencyUpdatedEvent>();
            }
        }
        public override void Initialize()
        {
            _eventService = GameManager.EventService;
        }
    }
}
