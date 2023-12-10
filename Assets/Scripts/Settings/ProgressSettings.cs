using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigames.Fight
{
    public enum ResourceType
    {
        Dirt,
        Grass, 
    }

    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/ProgressSettings", order = 1)]
    [Serializable]
    public class ProgressSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<World> Worlds;
        public Dictionary<TierCategory, float> UnlockCostMaps = new() 
        {
            {TierCategory.Tier1, 1},
            {TierCategory.Tier2, 2},
            {TierCategory.Tier3, 3},
        };
        [Header("Run-time Values")] 
        public float Dna;
        public float BankedDna;
        public TutorialState TutorialState;
        public ResourceTypeFloatDictionary PhysicalResources = new();
        public float BaseResourceValue;

        [SerializeField]
        private World currentWorld;

        public World CurrentWorld
        {
            get
            {
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
                    if (world.IsCompleted)
                        worldsCompleted++;
                }

                return worldsCompleted;
            }
        }

        [ContextMenu("Set Defaults")]
        public void SetDefaults()
        {
            CurrentWorld = null;
            Dna = 0;
            BankedDna = 0;
            BaseResourceValue = 1;
            TutorialState = TutorialState.None;

            foreach (var world in Worlds)
            {
                world.SetDefaults();
            }


            PhysicalResources = new();
            foreach (var enumValue in Enum.GetValues(typeof(ResourceType))){
                PhysicalResources[enumValue] = 0f;
            }
        }

        public void Init()
        {
            // TODO: don't just always pick the first world
            currentWorld = Worlds[0];   
        }
        
        public ProgressModel GetProgressForSerialization()
        {
            ProgressModel toReturn = new ProgressModel();

            toReturn.WorldData = GetWorldData();
            toReturn.Dna = Dna;
            toReturn.BankedDna = BankedDna;
            toReturn.TutorialState = TutorialState;
            toReturn.PhysicalResources = PhysicalResources;

            return toReturn;
        }

        public List<WorldData> GetWorldData()
        {
            List<WorldData> worldData = new List<WorldData>();

            foreach (var world in Worlds)
            {
                WorldData worldModel = new WorldData();

                worldModel.WorldName = world.Name;
                worldData.Add(worldModel);
            }

            return worldData;
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

    [Serializable]
    public class World
    {
        [Header("Set in Editor")] 
        public string Name;
        public Sprite WorldSprite;
        public RoomSettings RoomSettings;

        [Header("Run-time Values")]
        public bool IsUnlocked;
        public bool IsCompleted;

        public void SetDefaults()
        {
            IsUnlocked = false;
            IsCompleted = false;
        }
    }
}