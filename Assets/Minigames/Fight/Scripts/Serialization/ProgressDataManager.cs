using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Progress.dat";

    public static ProgressModel Load()
    {
        ProgressModel data = FileUtils.LoadFile<ProgressModel>(FileLocation);
        return data;
    }

    public static void Save()
    {
        ProgressModel data = GameManager.SettingsManager.GetProgressForSerialization();
        FileUtils.SaveFile(FileLocation, data);
    }
}
