using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerEntity playerPrefab;
        private PlayerEntity _playerEntity;
        [SerializeField] private CameraLerp cameraLerp;
        private CameraLerp _cameraLerp;
       // [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private DamageTextPool damageTextPool;
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private ShadowData shadowData;
    
        public static PlayerEntity PlayerEntity => Instance._playerEntity;
        public static CameraLerp CameraLerp => Instance._cameraLerp;
        // public static EnemySpawnManager EnemySpawnManager => Instance.enemySpawnManager;
        public static SettingsManager SettingsManager => Instance.settingsManager;
        public static CurrencyManager CurrencyManager => Instance.currencyManager;
        public static UIManager UIManager => Instance.uiManager;
        public static DamageTextPool DamageTextPool => Instance.damageTextPool;
        public static RoomManager RoomManager => Instance.roomManager;
        public static ShadowData ShadowData => Instance.shadowData;

        private float autoSaveTimer;
        private const float autoSaveInterval = 10;
        
        

        protected override void Initialize()
        {
            SettingsManager.Init();
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            _playerEntity = Instantiate(playerPrefab);
            _cameraLerp = Instantiate(cameraLerp);
        }

        private void Update()
        {
            autoSaveTimer += Time.deltaTime;

            if (autoSaveTimer > autoSaveInterval)
            {
                autoSaveTimer = 0;
                //Save();
            }
        }

        private void OnDestroy()
        {
            Save();
        
            // In the editor we want to clear scriptable object changes that way they aren't saved and always in the git history, and messing up tests
            // This isn't a problem in the built application as scriptable object changes don't save
#if UNITY_EDITOR
            SettingsManager.SetDefaults();
#endif
        }

        private void Save()
        {
            ProgressDataManager.Save(SettingsManager.progressSettings);
            EffectDataManager.Save();
        }
    }
}
