using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public class GameManager : MonoBehaviour
    {
        [Header("Set In Editor")] 
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private LauncherSettings _launcherSettings;
        [SerializeField] private ProjectileSettings _projectileSettings;
        [SerializeField] private Launcher _launcher;

        [Header("Debug")] 
        [SerializeField] private GameState _gameState;
        private Camera _mainCamera;
        private Vector3 _projectileFirePos;
        private Vector3 _touchStartPosition;
        private Vector3 _flingForceVector;


        [SerializeField] private List<Fish> _fishOnLure;
        private Lure _currentLure;

        private EventService _eventService;

        public enum GameState
        {
            WaitForSlingshot,
            SlingShotting,
            Fling,
            RoundOver,
            Reset
        }

        void Start()
        {
            _projectileFirePos = _launcher.FirePosition;
            _eventService = Services.Instance.EventService;
            SetState(GameState.WaitForSlingshot);
            _mainCamera = Camera.main;

            _eventService.Add<FishCaughtEvent>(FishWasCaught);
            _eventService.Add<ResetGameEvent>(ResetGame);
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
                    _eventService.Dispatch<SlingReadyWithProjectile>();
                    SetupProjectile();
                    break;
                case GameState.SlingShotting:
                    _eventService.Dispatch<BeginFlingEvent>();
                    break;
                case GameState.Fling:
                    _eventService.Dispatch(new LureThrownEvent(_currentLure));
                    _currentLure.Throw(_flingForceVector);
                    break;
                case GameState.RoundOver:
                    // TODO: show score
                    _eventService.Dispatch<EndGameEvent>();
                    break;
                case GameState.Reset:
                    // TODO: reset scene
                    SetState(GameState.WaitForSlingshot);
                    break;
            }
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
            angle = Mathf.Clamp(angle, 0, 75f);
            var mag = Mathf.Min(deltaTouch.magnitude, _launcherSettings.SlingshotMaxDistance);
            var fill = Mathf.Max(mag / _launcherSettings.SlingshotMaxDistance, 0.20f);
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var finalVector = rotation * Vector3.right; //Normalized

            _launcher.UpdateRotation(rotation, fill);

            _flingForceVector = finalVector * _launcherSettings.SlingshotStrength * fill;
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
        }
    }
}