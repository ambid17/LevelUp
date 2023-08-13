using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomPropGenerator))]
public class RoomPropGeneratorEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomPropGenerator myScript = (RoomPropGenerator)target;
        if (GUILayout.Button("Generate Props"))
        {
            myScript.GenerateProps();
        }
    }
}
