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
        [SerializeField] public EffectSettings effectSettings;
        [SerializeField] private Camera minimapCamera;
        private Camera _playerCamera;
        private Camera _minimapCamera;

        public static PlayerEntity PlayerEntity => Instance._playerEntity;
        public static CameraLerp CameraLerp => Instance._cameraLerp;
        public static CurrencyManager CurrencyManager => Instance.currencyManager;
        public static UIManager UIManager => Instance.uiManager;
        public static DamageTextPool DamageTextPool => Instance.damageTextPool;
        public static RoomManager RoomManager => Instance.roomManager;
        public static ShadowData ShadowData => Instance.shadowData;
        public static ProgressSettings ProgressSettings => Instance.progressSettings;
        public static EffectSettings EffectSettings => Instance.effectSettings;
        public static Camera PlayerCamera => Instance._playerCamera;
        public static Camera MinimapCamera => Instance._minimapCamera;

        protected override void Initialize()
        {
            progressSettings.Init();
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            _playerEntity = Instantiate(playerPrefab);
            _cameraLerp = Instantiate(cameraLerpPrefab);
            _playerCamera = _cameraLerp.GetComponent<Camera>();
            _minimapCamera = Instantiate(minimapCamera);
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
