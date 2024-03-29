using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Minigames.Fight;

public class DeleteAllPlayerPrefs : MonoBehaviour
{
    [MenuItem("Caos Creations/Delete save data")]
    static void DeleteSaveData()
    {
        var paths = Directory.EnumerateFiles(Application.persistentDataPath);
        foreach(var path in paths) 
        {
            File.Delete(path);
        }

        ResetScriptableObjects();
    }

    static void ResetScriptableObjects()
    {
        var progress = AssetDatabase.LoadAssetAtPath<ProgressSettings>("Assets/ScriptableObjects/ProgressSettings.asset");
        progress.SetDefaults();
        EditorUtility.SetDirty(progress);
        
        var effectSettings = AssetDatabase.LoadAssetAtPath<UpgradeSettings>("Assets/ScriptableObjects/UpgradeSettings.asset");
        effectSettings.SetDefaults();
        EditorUtility.SetDirty(effectSettings);
        
        AssetDatabase.SaveAssets();
    }
}
