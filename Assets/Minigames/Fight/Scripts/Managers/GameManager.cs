using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerEntity playerPrefab;
        private PlayerEntity _playerEntity;
        [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] private CurrencyManager currencyManager;
    
        public static PlayerEntity PlayerEntity => Instance._playerEntity;
        public static EnemySpawnManager EnemySpawnManager => Instance.enemySpawnManager;
        public static SettingsManager SettingsManager => Instance.settingsManager;
        public static CurrencyManager CurrencyManager => Instance.currencyManager;

        private float autoSaveTimer;
        private const float autoSaveInterval = 10;
        
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

        public override void Initialize()
        {
            SettingsManager.Init();
            SetupPlayer();
        }

        private void SetupPlayer()
        {
            _playerEntity = Instantiate(playerPrefab);
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
            ProgressDataManager.Save();
            UpgradeDataManager.Save();
        }
    }
}