using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [MenuItem("Caos Creations/Fight Settings _F2")]
    public static void SelectScriptableObjectFolder()
    {
        string folderPath = "Assets/Minigames/Fight/ScriptableObjects/EffectSettings.asset";
        var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(folderPath);
        EditorGUIUtility.PingObject(obj);
    }

    [MenuItem("Caos Creations/Open Save Folder _F5")]
    public static void OpenSaveFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
