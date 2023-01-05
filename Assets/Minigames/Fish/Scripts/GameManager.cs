using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
namespace Minigames.Fish
{
    public enum GameState
    {
        WaitForSlingshot,
        SlingShotting,
        Fling,
        RoundOver
    }
    
    public class GameManager : Singleton<GameManager>
    {
        [Header("Set In Editor")] 
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private LauncherSettings _launcherSettings;
        [SerializeField] private ProjectileSettings _projectileSettings;
        [SerializeField] private FishSettings _fishSettings;
        [SerializeField] private FishSpawnSettings _fishSpawnSettings;
        [SerializeField] private ProgressSettings _progressSettings;
        [SerializeField] private Launcher _launcher;
        [SerializeField] private FishSpawner _fishSpawner;


        public static float Currency
        {
            get => Instance._progressSettings.Currency;

            set
            {
                Instance._progressSettings.Currency = value;
                Instance._eventService.Dispatch<CurrencyUpdatedEvent>();
            }
        }

        public static float CurrentWeightPercentage =>
            Instance._fishOnLure.Sum(f => f.Weight) / Instance._launcherSettings.ReelMaxWeight;
        public static LauncherSettings LauncherSettings => Instance._launcherSettings;
        public static ProjectileSettings ProjectileSettings => Instance._projectileSettings;
        public static FishSettings FishSettings => Instance._fishSettings;
        public static FishSpawnSettings FishSpawnSettings => Instance._fishSpawnSettings;
        public static FishSpawner FishSpawner => Instance._fishSpawner;

        [Header("Debug")] 
        [SerializeField] private GameState _gameState;
        public static GameState GameState => Instance._gameState;
        
        private Camera _mainCamera;
        private Vector3 _projectileFirePos;
        private Vector3 _touchStartPosition;
        private Vector3 _flingForceVector;


        [SerializeField] private List<FishInstanceSettings> _fishOnLure;
        private float _weightOfFishOnLure;
        
        private Lure _currentLure;
        public static Lure CurrentLure => Instance._currentLure;

        private EventService _eventService;

        

        void Start()
        {
            _projectileFirePos = _launcher.FirePosition;
            _eventService = Services.Instance.EventService;
            SetState(GameState.WaitForSlingshot);
            _mainCamera = Camera.main;

            _eventService.Add<FishCaughtEvent>(FishWasCaught);
            _eventService.Add<ReeledInEvent>( () => SetState(GameState.RoundOver) );
        }

        void Update()
        {
            switch (_gameState)
            {
                case GameState.WaitForSlingshot:
                    if (DidStartSlingshot())
                    {
                        SetState(GameState.SlingShotting);
                    }

                    break;
                case GameState.SlingShotting:
                    UpdateLurePosition();

                    if (Input.GetMouseButtonUp(0))
                    {
                        SetState(GameState.Fling);
                    }

                    break;
            }
        }

        public void SetState(GameState newState)
        {
            _gameState = newState;

            switch (newState)
            {
                case GameState.WaitForSlingshot:
                    _eventService.Dispatch<WaitingForSlingshotEvent>();
                    SetupProjectile();
                    break;
                case GameState.SlingShotting:
                    break;
                case GameState.Fling:
                    _eventService.Dispatch(new LureThrownEvent(_currentLure));
                    _currentLure.Throw(_flingForceVector);
                    _launcher.ClearTrajectory();
                    break;
                case GameState.RoundOver:
                    AddFishToInventory();
                    SetState(GameState.WaitForSlingshot);
                    break;
            }
        }

        void AddFishToInventory()
        {
            foreach (var fishInstance in _fishOnLure)
            {
                _fishSettings.AddFish(fishInstance);
            }

            _fishOnLure.Clear();
            _weightOfFishOnLure = 0;
            _eventService.Dispatch<FishOnLureUpdatedEvent>();
        }

        void SetupProjectile()
        {
            GameObject projectileGO = Instantiate(projectilePrefab);
            _currentLure = projectileGO.GetComponent<Lure>();
            _currentLure.Setup(_projectileSettings.CurrentProjectile);
        }

        bool DidStartSlingshot()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _touchStartPosition = GetMouseWorldPosition();
                return true;
            }

            return false;
        }

        void UpdateLurePosition()
        {
            var currentTouchPosition = GetMouseWorldPosition();
            var touchDelta = _touchStartPosition - currentTouchPosition;

            var angle = Vector3.SignedAngle(Vector3.right, touchDelta, Vector3.forward);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var finalVector = rotation * Vector3.right; // Get the Normalized x component

            _launcher.UpdateRotation(rotation);

            _flingForceVector = finalVector * _launcherSettings.SlingshotStrength;
            _launcher.UpdateTrajectory(_projectileFirePos, _flingForceVector);
        }

        private Vector3 GetMouseWorldPosition()
        {
            var worldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = _projectileFirePos.z;
            return worldPos;
        }

        public void FishWasCaught(FishCaughtEvent eventType)
        {
            _fishOnLure.Add(eventType.Fish);
            _weightOfFishOnLure += eventType.Fish.Weight;

            if (_weightOfFishOnLure > _launcherSettings.ReelMaxWeight)
            {
                _fishOnLure.Clear();
                _weightOfFishOnLure = 0;
                _eventService.Dispatch<LureSnappedEvent>();
                SetState(GameState.RoundOver);
            }
            
            _eventService.Dispatch<FishOnLureUpdatedEvent>();
        }
    }
}