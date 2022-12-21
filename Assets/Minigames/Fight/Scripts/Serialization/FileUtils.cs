using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileUtils
{
    public static T LoadFile<T>(string fileLocation)
    {
        if (File.Exists(fileLocation))
        {
            string fileData = File.ReadAllText(fileLocation);
            T toReturn = JsonUtility.FromJson<T>(fileData);
            return toReturn;
        }

        return default(T);
    }

    public static void SaveFile<T>(string filePath, T objectToSerialize)
    {
        string fileContent = JsonUtility.ToJson(objectToSerialize);
        File.WriteAllText(filePath, fileContent);
    }
}
