using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class SettingsManager : MonoBehaviour
    {
        public ProgressSettings progressSettings;
        public WeaponSettings weaponSettings;
        public EffectSettings effectSettings;
        public PlayerSettings playerSettings;
        public EnemySpawnerSettings enemySpawnerSettings;
        public IncomeSettings incomeSettings;

        private EventService _eventService;
        void Start()
        {
            _eventService = GameManager.EventService;
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
    }
}
