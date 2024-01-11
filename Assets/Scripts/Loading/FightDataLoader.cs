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

    // List of entities that have their entityStats serialized
    [SerializeField]
    private List<Entity> serializableEntities;
    public static List<Entity> SerializableEntities => Instance.serializableEntities;

    // The model that holds the data for remapping entityStats to a new file
    [SerializeField]
    public SerializableDictionary<string, string> fileNameRemappings;
    public static SerializableDictionary<string, string> FileNameRemappings => Instance.fileNameRemappings;

    // Maps from the entity's file name to the entity stats so it can be loaded once an accessed instantly
    [SerializeField]
    private SerializableDictionary<string, EntityStats> entityStatsMap;
    public static SerializableDictionary<string, EntityStats> EntityStatsMap => Instance.entityStatsMap;

    


    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
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
            fileNameRemappings = new();

            foreach(var entity in serializableEntities)
            {
                fileNameRemappings.Add(entity.statsFileName, entity.statsFileName);
            }
            FileUtils.SaveFile(EntityStatsRemapModel.FILE_PATH, new EntityStatsRemapModel(fileNameRemappings));
        }
        else
        {
            var model = FileUtils.LoadFile<EntityStatsRemapModel>(EntityStatsRemapModel.FILE_PATH);
            fileNameRemappings = model.fileNameRemappings;
        }
    }

    public void SaveEntityStatsRemapping()
    {
        FileUtils.SaveFile(EntityStatsRemapModel.FILE_PATH, new EntityStatsRemapModel(fileNameRemappings));
    }

    public void UpdateRemapping(string baseName, string remappedName)
    {
        fileNameRemappings[baseName] = remappedName;

        var entityToUpdate = serializableEntities.First(e => e.statsFileName == baseName);
        
        // Update the stats mapping
        var stats = FileUtils.LoadFile<EntityStats>(GetEntityMappedFilePath(entityToUpdate));
        entityStatsMap[entityToUpdate.statsFileName] = stats;

        // save the stat remapping
        FileUtils.SaveFile(EntityStatsRemapModel.FILE_PATH, new EntityStatsRemapModel(fileNameRemappings));

        // Send out an event so entities can choose to use the new mapping
        Platform.EventService.Dispatch(new EntityStatsFileRemappedEvent(baseName));
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
            var defaultFilePath = GetEntityDefaultFilePath(entity);
            if (!File.Exists(defaultFilePath))
            {
                var entityStats = entity.GetComponent<Entity>().Stats;
                FileUtils.SaveFile<EntityStats>(defaultFilePath, entityStats);
            }
        }
    }

    private void LoadEntityStats()
    {
        entityStatsMap = new();
        foreach (var entity in serializableEntities)
        {
            var filePath = GetEntityMappedFilePath(entity);
            var stats = FileUtils.LoadFile<EntityStats>(filePath);
            entityStatsMap.Add(entity.statsFileName, stats);
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
        
        for (int worldIndex = 0; worldIndex < progressModel.BiomeData.Count; worldIndex++)
        {
            Platform.ProgressSettings.Biomes[worldIndex].FloorsCompleted = progressModel.BiomeData[worldIndex].FloorsCompleted;
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

    public static string GetEntityFolderPath()
    {
        return Path.Combine(Application.persistentDataPath, FightDataLoader.ENTITY_FOLDER);
    }

    public static string GetEntityDefaultFilePath(Entity entity)
    {
        return Path.Combine(GetEntityFolderPath(), GetEntityDefaultFileName(entity));
    }

    public static string GetEntityDefaultFileName(Entity entity)
    {
        return $"{entity.statsFileName}.json";
    }

    public static string GetEntityMappedFilePath(Entity entity)
    {
        return Path.Combine(GetEntityFolderPath(), GetEntityMappedFileName(entity));
    }

    public static string GetEntityMappedFileName(Entity entity)
    {
        var fileName = Instance.fileNameRemappings[entity.statsFileName];
        return $"{fileName}.json"; ;
    }
}
