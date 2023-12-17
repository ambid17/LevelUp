using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Minigames.Fight;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightDataLoader : Singleton<FightDataLoader>
{
    public EffectSettings effectSettings;
    public static int TargetSceneIndex;
    public static readonly string ENTITY_FOLDER = "Entities";

    [SerializeField]
    private List<Entity> serializableEntities;
    public static List<Entity> SerializableEntities => Instance.serializableEntities;

    [SerializeField]
    private EntityStatsRemapModel entityStatsRemapModel;
    public static EntityStatsRemapModel EntityStatsRemapModel => Instance.entityStatsRemapModel;

    [SerializeField]
    private SerializableDictionary<Entity, EntityStats> entityStatsMap;
    public static SerializableDictionary<Entity, EntityStats> EntityStatsMap => Instance.entityStatsMap;



    private void Awake()
    {
        Scene current = SceneManager.GetActiveScene();
        if (current.buildIndex == AnySceneLaunch.ANY_SCENE_LAUNCH_INDEX)
        {
            Load();
            SceneManager.LoadScene(TargetSceneIndex);
        }
    }

    public void Load()
    {
        LoadEntityStatsRemapping();
        CreateEntityStatsFiles();
        LoadEntityStats();
        LoadProgressData();
        LoadEffectData();
    }

    private void LoadEntityStatsRemapping()
    {
        // handle the file on first load where it doesn't exist
        // just create the file and remap all entityStats to their defaults
        if (!File.Exists(EntityStatsRemapModel.FILE_PATH))
        {
            entityStatsRemapModel = new EntityStatsRemapModel();

            foreach(var entity in serializableEntities)
            {
                entityStatsRemapModel.fileNameRemappings.Add(entity.statsFileName, entity.statsFileName);
            }
            FileUtils.SaveFile(EntityStatsRemapModel.FILE_PATH, entityStatsRemapModel);
        }
        else
        {
            entityStatsRemapModel = FileUtils.LoadFile<EntityStatsRemapModel>(EntityStatsRemapModel.FILE_PATH);
        }
    }

    /// <summary>
    /// This relies on serializableEntities being set up from the context menu function: PopulateSerializableEntities()
    /// </summary>
    private void CreateEntityStatsFiles()
    {
        var dataFolder = Application.persistentDataPath;
        var entitiesFolder = Path.Combine(dataFolder, FightDataLoader.ENTITY_FOLDER);

        if (!Directory.Exists(entitiesFolder))
        {
            Directory.CreateDirectory(entitiesFolder);
        }

        foreach (var entity in serializableEntities)
        {
            if (!File.Exists(entity.ENTITY_STATS_FILE_PATH))
            {
                var entityStats = entity.GetComponent<Entity>().Stats;
                FileUtils.SaveFile<EntityStats>(entity.ENTITY_STATS_FILE_PATH, entityStats);
            }
        }
    }

    private void LoadEntityStats()
    {
        entityStatsMap = new();
        foreach (var entity in serializableEntities)
        {
            var stats = FileUtils.LoadFile<EntityStats>(entity.ENTITY_STATS_FILE_PATH);
            entityStatsMap.Add(entity, stats);
        }
    }

    private void LoadEffectData()
    {
        var effectContainer = EffectDataManager.Load();
        if (effectContainer != null && effectContainer.upgrades != null)
        {
            foreach (var upgrade in effectContainer.upgrades)
            {
                effectSettings.LoadSavedUpgrade(upgrade);
            }
        }
    }
    
    public void LoadProgressData()
    {
        ProgressModel data = ProgressDataManager.Load();

        if (data != null)
        {
            LoadSerializedProgress(data);
        }
        
        Platform.ProgressSettings.UnlockWorlds();
    }
    
    public void LoadSerializedProgress(ProgressModel progressModel)
    {
        Platform.ProgressSettings.Dna = progressModel.Dna;
        Platform.ProgressSettings.BankedDna = progressModel.BankedDna;
        Platform.ProgressSettings.PhysicalResources = progressModel.PhysicalResources;
        
        for (int worldIndex = 0; worldIndex < progressModel.WorldData.Count; worldIndex++)
        {
            Platform.ProgressSettings.Worlds[worldIndex].IsUnlocked = progressModel.WorldData[worldIndex].IsUnlocked;
            Platform.ProgressSettings.Worlds[worldIndex].IsCompleted = progressModel.WorldData[worldIndex].IsCompleted;
        }
    }


    /// <summary>
    /// In order for this to work, you have to set the "Asset Label" of each prefab to "Entity"
    /// </summary>
    [ContextMenu("Populate SerializableEntities")]
    public void PopulateSerializableEntities()
    {
        string[] entityGuids = AssetDatabase.FindAssets("l:Entity");
        serializableEntities = new();

        foreach (string guid in entityGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject entityPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
            Entity entity = entityPrefab.GetComponent<Entity>();

            serializableEntities.Add(entity);
        }

        EditorUtility.SetDirty(this);
    }

    
}
