using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Progress.dat";

    public static void LoadAndApplyData()
    {
        ProgressModel data = FileUtils.LoadFile<ProgressModel>(FileLocation);
        GameManager.Instance.ApplyProgress(data);

    }

    public static void Save()
    {
        ProgressModel data = GameManager.Instance.GetProgress();
        FileUtils.SaveFile(FileLocation, data);
    }
}
