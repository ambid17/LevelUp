using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntityStatsEditor : MonoBehaviour
{
    public enum EditorMode
    {
        Edit,
        Duplicate
    }

    private EditorMode _editorMode;
    private EntityStats _entityStats;

    void Start()
    {
        
    }

    public void Setup(string fileName, EditorMode editorMode)
    {
        _editorMode = editorMode;
        _entityStats = GetFileData(fileName);
    }

    private EntityStats GetFileData(string fileName)
    {
        var dataFolder = Application.persistentDataPath;
        var entitiesFolder = Path.Combine(dataFolder, "Entities");

        if (!Directory.Exists(entitiesFolder))
        {
            Directory.CreateDirectory(entitiesFolder);
        }

        var entityFilePath = Path.Combine(entitiesFolder, fileName);

        if(!File.Exists(entityFilePath))
        {
            Debug.LogError($"EntityStats file not found at path: {entityFilePath}");
            return null;
        }

        return FileUtils.LoadFile<EntityStats>(entityFilePath);
    }
}
