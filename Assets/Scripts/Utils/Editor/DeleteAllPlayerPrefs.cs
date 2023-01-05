using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DeleteAllPlayerPrefs : MonoBehaviour
{
    [MenuItem("Tools/Caos Creations/Delete save data")]
    static void DeletePlayerPrefs()
    {
        var paths = Directory.EnumerateFiles(Application.persistentDataPath);
        foreach(var path in paths) 
        {
            File.Delete(path);
        }
    }
}
