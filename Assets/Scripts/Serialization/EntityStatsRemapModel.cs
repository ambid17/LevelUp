using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EntityStatsRemapModel
{
    public static readonly string FILE_NAME = "EntityStatsRemap";
    public static string FILE_PATH => Path.Combine(Application.persistentDataPath, $"{FILE_NAME}.json");
    /// <summary>
    /// This is used to allow the player to overwrite what file is used for each entity's stats.
    /// 
    /// For example, if the player entity's file name is "Player.json", 
    /// and you want to instead use the "Player2.json" stats, the dictionary entry should be:
    /// {"Player", "Player2"}
    /// </summary>
    public SerializableDictionary<string, string> fileNameRemappings;

    public EntityStatsRemapModel()
    {
        fileNameRemappings = new();
    }

    public EntityStatsRemapModel(SerializableDictionary<string, string> fileNameRemappings)
    {
        this.fileNameRemappings = fileNameRemappings;
    }
}
