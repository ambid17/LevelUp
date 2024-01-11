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
        [Header("Set in Editor")] public List<Biome> Biomes;
        public Dictionary<TierCategory, float> UnlockCostMaps = new() 
        {
            {TierCategory.Tier1, 1},
            {TierCategory.Tier2, 2},
            {TierCategory.Tier3, 3},
        };
        [Header("Run-time Values")] 
        public float Dna;
        public float BankedDna;
        public ResourceTypeFloatDictionary PhysicalResources = new();
        public float BaseResourceValue;

        [SerializeField]
        private Biome currentBiome;

        public Biome CurrentBiome
        {
            get
            {
                return currentBiome;
            }
            set { currentBiome = value; }
        }

        public int BiomesCompleted
        {
            get
            {
                int biomesCompleted = 0;

                foreach (var biome in Biomes)
                {
                    if (biome.IsCompleted)
                        biomesCompleted++;
                }

                return biomesCompleted;
            }
        }

        [ContextMenu("Set Defaults")]
        public void SetDefaults()
        {
            CurrentBiome = null;
            Dna = 0;
            BankedDna = 0;
            BaseResourceValue = 1;

            foreach (var world in Biomes)
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
            currentBiome = Biomes[0];   
        }
        
        public ProgressModel GetProgressForSerialization()
        {
            ProgressModel toReturn = new ProgressModel();

            toReturn.BiomeData = GetBiomeData();
            toReturn.Dna = Dna;
            toReturn.BankedDna = BankedDna;
            toReturn.PhysicalResources = PhysicalResources;

            return toReturn;
        }

        public List<BiomeData> GetBiomeData()
        {
            List<BiomeData> biomeData = new List<BiomeData>();

            foreach (var biome in Biomes)
            {
                BiomeData biomeModel = new BiomeData();

                biomeModel.BiomeName = biome.Name;
                biomeModel.FloorsCompleted = biome.FloorsCompleted;
                biomeData.Add(biomeModel);
            }

            return biomeData;
        }

        public void UnlockWorlds()
        {
            int conquered = BiomesCompleted;

            for (int i = 0; i < Biomes.Count; i++)
            {
                Biomes[i].IsUnlocked = conquered >= i;
            }
        }

        public void CompleteFloor()
        {
            currentBiome.FloorsCompleted++;
        }
    }

    [Serializable]
    public class Biome
    {
        [Header("Set in Editor")] 
        public string Name;
        public Sprite BiomeSprite;
        public RoomSettings RoomSettings;
        public int FloorsToComplete;

        [Header("Run-time Values")]
        public bool IsUnlocked;
        public int FloorsCompleted;
        public bool IsCompleted => FloorsCompleted >= FloorsToComplete;

        public void SetDefaults()
        {
            IsUnlocked = false;
            FloorsCompleted = 0;
        }
    }
}