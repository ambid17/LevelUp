using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerEntity playerPrefab;
        private PlayerEntity _playerEntity;
        [SerializeField] private CameraLerp cameraLerpPrefab;
        private CameraLerp _cameraLerp;
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private DamageTextPool damageTextPool;
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private ShadowData shadowData;
        [SerializeField] public ProgressSettings progressSettings;
        [SerializeField] public UpgradeSettings effectSettings;
        [SerializeField] private Camera minimapCameraPrefab;
        [SerializeField] private EnemyObjectPool enemyObjectPoolPrefab;
        private Camera _playerCamera;
        private Camera _minimapCamera;
        private EnemyObjectPool _enemyObjectPool;

        public static PlayerEntity PlayerEntity => Instance._playerEntity;
        public static CameraLerp CameraLerp => Instance._cameraLerp;
        public static CurrencyManager CurrencyManager => Instance.currencyManager;
        public static UIManager UIManager => Instance.uiManager;
        public static DamageTextPool DamageTextPool => Instance.damageTextPool;
        public static RoomManager RoomManager => Instance.roomManager;
        public static ShadowData ShadowData => Instance.shadowData;
        public static ProgressSettings ProgressSettings => Instance.progressSettings;
        public static UpgradeSettings EffectSettings => Instance.effectSettings;
        public static Camera PlayerCamera => Instance._playerCamera;
        public static Camera MinimapCamera => Instance._minimapCamera;
        public static EnemyObjectPool EnemyObjectPool => Instance._enemyObjectPool;

        protected override void Initialize()
        {
            progressSettings.Init();
            SetupPlayer();

            // Spawn the object pool arbitrarily far away to ensure it doesn't mess with calculations.
            _enemyObjectPool = Instantiate(enemyObjectPoolPrefab, new Vector2(1000,1000), Quaternion.identity);
        }

        private void SetupPlayer()
        {
            _playerEntity = Instantiate(playerPrefab);
            _cameraLerp = Instantiate(cameraLerpPrefab);
            _playerCamera = _cameraLerp.GetComponent<Camera>();
            _minimapCamera = Instantiate(minimapCameraPrefab);
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            progressSettings.SetDefaults();
            effectSettings.SetDefaults();
        }
#endif
    }
}
