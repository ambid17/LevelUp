using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatsEditor : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private DevCheatsUI devCheatsUI;
    private EntityStats _entityStats;

    void Start()
    {
        saveButton.onClick.AddListener(Save);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Save()
    {
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }

    private void Cancel()
    {
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }

    public void Setup(Entity entityToEdit)
    {
        visuals.SetActive(true);
        _entityStats = FightDataLoader.EntityStatsMap[entityToEdit.statsFileName];
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
