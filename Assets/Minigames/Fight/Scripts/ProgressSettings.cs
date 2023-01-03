using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/ProgressSettings", order = 1)]
    [Serializable]
    public class ProgressSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<World> Worlds;

        [Header("Run-time Values")] public float Currency;

        private World _currentWorld;

        public World CurrentWorld
        {
            get
            {
#if UNITY_EDITOR
                // Allow the game scenes to be played not from the main menu
                if (string.IsNullOrEmpty(_currentWorld.Name))
                {
                    int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
                    _currentWorld = Worlds.First(world => world.SkillingSceneIndex == currentBuildIndex);
                }
#endif
                return _currentWorld;
            }
            set { _currentWorld = value; }
        }

        public void SetDefaults()
        {
            CurrentWorld = null;
            Currency = 0;

            foreach (var world in Worlds)
            {
                world.SetDefaults();
            }
        }

        public void Init()
        {
            Country highestCountry = CurrentWorld.Countries[0];

            for (int i = CurrentWorld.Countries.Count - 1; i >= 0; i--)
            {
                if (CurrentWorld.Countries[i].EnemyKillCount > 0)
                {
                    highestCountry = CurrentWorld.Countries[i];
                    break;
                }
            }

            CurrentWorld.CurrentCountry = highestCountry;
        }

        public List<WorldData> GetWorldData()
        {
            List<WorldData> worldData = new List<WorldData>();

            foreach (var world in Worlds)
            {
                WorldData worldModel = new WorldData();

                worldModel.WorldName = world.Name;
                worldModel.CountryData = world.GetCountryData();
                worldModel.LastTimeVisited = world == CurrentWorld ? DateTime.Now : world.LastTimeVisited;
                worldModel.CurrencyPerMinute = world.CurrencyPerMinute;

                worldData.Add(worldModel);
            }

            return worldData;
        }

        public void AddKill()
        {
            CurrentWorld.CurrentCountry.EnemyKillCount++;
        }
    }

    public enum WorldType
    {
        Fighting,
        Crafting,
        Engineering,
        Fishing,
        Mining
    }

    [Serializable]
    public class World
    {
        [Header("Set in Editor")] public string Name;
        public int SkillingSceneIndex;
        public Sprite WorldSprite;
        public List<Country> Countries;
        public List<Enemy> Enemies;
        public WorldType WorldType;

        [Header("Run-time Values")] public float CurrencyPerMinute;
        public DateTime LastTimeVisited;
        public Country CurrentCountry;
        public bool IsFighting;


        public void SetDefaults()
        {
            CurrencyPerMinute = 0;
            LastTimeVisited = DateTime.Now;
            CurrentCountry = null;

            foreach (var country in Countries)
            {
                country.SetDefaults();
            }
        }

        public List<CountryData> GetCountryData()
        {
            List<CountryData> countryData = new List<CountryData>();

            foreach (var country in Countries)
            {
                CountryData countryModel = new CountryData();
                countryModel.Kills = country.EnemyKillCount;
                countryModel.CountryIndex = country.Index;
                countryData.Add(countryModel);
            }

            return countryData;
        }

        public void TrySetPreviousCountry()
        {
            if (CurrentCountry.Index > 0)
            {
                CurrentCountry = Countries[CurrentCountry.Index - 1];
            }
        }

        public void TrySetNextCountry()
        {
            if (CurrentCountry.IsConquered)
            {
                CurrentCountry = Countries[CurrentCountry.Index + 1];
            }
        }

        public int GetConqueredCountryCount()
        {
            int count = 0;
            foreach (var country in Countries)
            {
                if (country.IsConquered)
                {
                    count++;
                }
            }

            return count;
        }


        // See this for more info:
        // https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
        [NonSerialized] private int _weightTotal;

        public Enemy GetRandomEnemy()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = Enemies.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var enemy in Enemies)
            {
                randomWeight -= enemy.SpawnWeight;
                if (randomWeight < 0)
                {
                    return enemy;
                }
            }

            return Enemies[0];
        }
    }

    [Serializable]
    public class Country
    {
        [Header("Set in Editor")] public int Index;
        public int EnemyKillsToComplete;
        public float EnemyStatScalar;
        public List<Vector2> SpritePixels;
        public Color EnemyTierColor;

        [Header("Run-time Values")] public int EnemyKillCount;

        public bool IsConquered => EnemyKillCount >= EnemyKillsToComplete;
        public float ConquerPercent => (float)EnemyKillCount / EnemyKillsToComplete;

        public void SetDefaults()
        {
            EnemyKillCount = 0;
        }
    }

    [Serializable]
    public class Enemy
    {
        public GameObject Prefab;
        public int SpawnWeight;
        public EnemyInstanceSettings Settings;
    }

    [Serializable]
    public class EnemyInstanceSettings
    {
        public float goldValue;
        public float shotSpeed;
        public float moveSpeed;
        public float maxHp = 100;
        public float weaponDamage;

        public float GoldValue =>
            goldValue * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar;

        public float ShotSpeed => shotSpeed;
        public float MoveSpeed => moveSpeed;

        public float MaxHp =>
            maxHp * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar;

        public float WeaponDamage => weaponDamage *
                                     GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry
                                         .EnemyStatScalar;
    }
}