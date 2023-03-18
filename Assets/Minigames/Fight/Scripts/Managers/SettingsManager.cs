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
        public UpgradeSettings upgradeSettings;
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
            upgradeSettings.SetDefaults();
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
            //effectSettings.UnlockAllEffects();
        }

        // private void OnUpgradePurchased(EffectPurchasedEvent eventType)
        // {
        //     Upgrade upgrade = eventType.Upgrade;
        //     switch (upgrade)
        //     {
        //         case PlayerUpgrade playerUpgrade:
        //             playerSettings.ApplyUpgrade(playerUpgrade);
        //             break;
        //         case WeaponUpgrade weaponUpgrade:
        //             weaponSettings.ApplyUpgrade(weaponUpgrade);
        //             break;
        //         case EnemyUpgrade enemyUpgrade:
        //             enemySpawnerSettings.ApplyUpgrade(enemyUpgrade);
        //             break;
        //         case IncomeUpgrade incomeUpgrade:
        //             incomeSettings.ApplyUpgrade(incomeUpgrade);
        //             break;
        //         default:
        //             Debug.LogError($"Invalid upgrade type: {upgrade.name}");
        //             break;
        //     }
        // }

        public List<Upgrade> GetAllUpgrades()
        {
            List<Upgrade> toReturn = new List<Upgrade>();

            toReturn.AddRange(upgradeSettings.PlayerUpgrades);
            toReturn.AddRange(upgradeSettings.WeaponUpgrades);
            toReturn.AddRange(upgradeSettings.EnemyUpgrades);
            toReturn.AddRange(upgradeSettings.IncomeUpgrades);

            return toReturn;
        }

        public List<Effect> GetAllEffects()
        {
            return effectSettings.UnlockedEffects;
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
