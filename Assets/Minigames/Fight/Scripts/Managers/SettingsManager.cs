using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class SettingsManager : MonoBehaviour
    {
        public PlayerSettings playerSettings;
        public WeaponSettings weaponSettings;
        public EffectSettings effectSettings;
        public ProgressSettings progressSettings;
        public EnemySpawnerSettings enemySpawnerSettings;
        public IncomeSettings incomeSettings;

        public int UpgradePurchaseCount;

        private EventService _eventService;
        void Start()
        {
            _eventService = GameManager.EventService;
            //_eventService.Add<EffectPurchasedEvent>(OnUpgradePurchased);
        }

        /// <summary>
        /// Reset settings to empty run-time stats where necessary.
        /// </summary>
        public void SetDefaults()
        {
            progressSettings.SetDefaults();
            weaponSettings.SetDefaults();
            effectSettings.SetDefaults();
        }

        /// <summary>
        /// Sets up all of the scriptable object to have base values
        /// </summary>
        public void Init()
        {
            playerSettings.Init();
            weaponSettings.Init();
            progressSettings.Init();
            enemySpawnerSettings.Init();
            incomeSettings.Init();
        }

        public ProgressModel GetProgressForSerialization()
        {
            ProgressModel toReturn = new ProgressModel();

            toReturn.WorldData = progressSettings.GetWorldData();
            toReturn.Currency = progressSettings.Currency;

            return toReturn;
        }
    }
}
