using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigames.Fight
{
    public enum CompletionType
    {
        None,
        World,
        Country
    }
    
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/ProgressSettings", order = 1)]
    [Serializable]
    public class ProgressSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<World> Worlds;

        [Header("Run-time Values")] 
        public float Currency;
        public TutorialState TutorialState;

        [SerializeField]
        private World currentWorld;

        public World CurrentWorld
        {
            get
            {
                if (currentWorld == null)
                {
                    return null;
                }
#if UNITY_EDITOR
                // Allow the game scenes to be played not from the main menu
                if (string.IsNullOrEmpty(currentWorld.Name))
                {
                    int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
                    
                    currentWorld = Worlds.FirstOrDefault(world => world.SkillingSceneIndex == currentBuildIndex);

                    if (currentWorld == null)
                    {
                        currentWorld = Worlds[0];
                    }
                }
#endif
                return currentWorld;
            }
            set { currentWorld = value; }
        }

        public int WorldsConquered
        {
            get
            {
                int worldsCompleted = 0;

                foreach (var world in Worlds)
                {
                    if (world.IsConquered())
                        worldsCompleted++;
                }

                return worldsCompleted;
            }
        }

        public void SetDefaults()
        {
            CurrentWorld = null;
            Currency = 0;
            TutorialState = TutorialState.None;

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
        
        public ProgressModel GetProgressForSerialization()
        {
            ProgressModel toReturn = new ProgressModel();

            toReturn.WorldData = GetWorldData();
            toReturn.Currency = Currency;
            toReturn.TutorialState = TutorialState;

            return toReturn;
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

        public CompletionType AddKill()
        {
            bool worldWasConquered = CurrentWorld.IsConquered();
            CompletionType completionType = CurrentWorld.CurrentCountry.AddKills(GameManager.SettingsManager.incomeSettings.KillsPerKill);

            if (CurrentWorld.IsConquered() && !worldWasConquered)
            {
                completionType = CompletionType.World;
            }

            return completionType;
        }

        public void ResetOnDeath()
        {
            if (!CurrentWorld.CurrentCountry.IsConquered)
            {
                CurrentWorld.CurrentCountry.EnemyKillCount = 0;
            }
        }

        public void UnlockWorlds()
        {
            int conquered = WorldsConquered;

            for (int i = 0; i < Worlds.Count; i++)
            {
                Worlds[i].IsUnlocked = conquered >= i;
            }
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
        [Header("Set in Editor")] 
        public string Name;
        public int SkillingSceneIndex;
        public Sprite WorldSprite;
        public List<Country> Countries;
        public List<Enemy> Enemies;
        public WorldType WorldType;

        [Header("Run-time Values")] 
        public float CurrencyPerMinute;
        public DateTime LastTimeVisited;
        public Country CurrentCountry;
        public bool IsFighting;
        public bool IsUnlocked;


        public void SetDefaults()
        {
            CurrencyPerMinute = 0;
            LastTimeVisited = DateTime.Now;
            CurrentCountry = null;
            IsFighting = false;
            IsUnlocked = false;

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

        public bool IsConquered()
        {
            return Countries.TrueForAll(country => country.IsConquered);
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

        [Header("Run-time Values")] public float EnemyKillCount;

        public bool IsConquered => EnemyKillCount >= EnemyKillsToComplete;
        public float ConquerPercent => (float)EnemyKillCount / EnemyKillsToComplete;

        /// <summary>
        /// Adds kills and returns whether those kills completed the country 
        /// </summary>
        public CompletionType AddKills(float kills)
        {
            bool wasConquered = IsConquered;

            EnemyKillCount += kills;

            return IsConquered && !wasConquered ? CompletionType.Country : CompletionType.None;
        }

        public void SetDefaults()
        {
            EnemyKillCount = 0;
        }
    }
}