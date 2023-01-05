using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using Utils;
namespace Minigames.Fish
{
    public enum GameState
    {
        WaitForSlingshot,
        SlingShotting,
        Fling,
        RoundOver,
        Reset
    }
    
    public class GameManager : Singleton<GameManager>
    {
        [Header("Set In Editor")] 
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private LauncherSettings _launcherSettings;
        [SerializeField] private ProjectileSettings _projectileSettings;
        [SerializeField] private FishSettings _fishSettings;
        [SerializeField] private ProgressSettings _progressSettings;
        [SerializeField] private Launcher _launcher;


        public static float Currency => Instance._progressSettings.Currency;

        public static float CurrentWeightPercentage =>
            Instance._fishOnLure.Sum(f => f.Weight) / Instance._launcherSettings.ReelMaxWeight;
        public static LauncherSettings LauncherSettings => Instance._launcherSettings;
        public static ProjectileSettings ProjectileSettings => Instance._projectileSettings;
        public static FishSettings FishSettings => Instance._fishSettings;

        [Header("Debug")] 
        [SerializeField] private GameState _gameState;
        public static GameState GameState => Instance._gameState;
        
        private Camera _mainCamera;
        private Vector3 _projectileFirePos;
        private Vector3 _touchStartPosition;
        private Vector3 _flingForceVector;


        [SerializeField] private List<FishInstanceSettings> _fishOnLure;
        
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
            _eventService.Add<ResetGameEvent>(ResetGame);
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
                case GameState.Fling:
                    if (Input.GetMouseButtonDown(0) && _currentLure != null)
                    {
                        //_currentProjectile.UseAbility();
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

            _fishOnLure = new List<FishInstanceSettings>();
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
            if (Input.GetMouseButtonDown(0))
            {
                var position = GetMouseWorldPosition();

                if (Vector3.SqrMagnitude(position - _projectileFirePos) < 25f)
                {
                    _touchStartPosition = Input.mousePosition / Screen.height;
                    return true;
                }
            }

            return false;
        }

        void EndFling()
        {
            if (_gameState == GameState.Fling)
            {
                SetState(GameState.WaitForSlingshot);
            }
        }

        private void ResetGame()
        {
            SetState(GameState.Reset);
        }

        void UpdateLurePosition()
        {
            var touchPos = Input.mousePosition / Screen.height;
            var deltaTouch = _touchStartPosition - touchPos;

            var angle = Vector3.SignedAngle(Vector3.right, deltaTouch, Vector3.forward);
            //angle = Mathf.Clamp(angle, 0, 75f);
            var mag = Mathf.Min(deltaTouch.magnitude, _launcherSettings.SlingshotMaxDistance);
            var fill = Mathf.Max(mag / _launcherSettings.SlingshotMaxDistance, 0.20f);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var finalVector = rotation * Vector3.right; //Normalized

            _launcher.UpdateRotation(rotation, fill);

            _flingForceVector = finalVector * _launcherSettings.SlingshotStrength * fill;
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
            _eventService.Dispatch<FishOnLureUpdatedEvent>();
        }
    }
}