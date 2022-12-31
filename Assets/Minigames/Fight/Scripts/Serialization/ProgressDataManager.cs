using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Progress.dat";

    public static void LoadAndApplyData()
    {
        ProgressModel data = FileUtils.LoadFile<ProgressModel>(FileLocation);

        if (data != null)
        {
            GameManager.SettingsManager.LoadSerializedProgress(data);
        }
    }

    public static void Save()
    {
        ProgressModel data = GameManager.SettingsManager.GetProgressForSerialization();
        FileUtils.SaveFile(FileLocation, data);
    }
}
