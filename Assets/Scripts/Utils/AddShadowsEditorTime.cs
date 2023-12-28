#if UNITY_EDITOR
using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddShadowsEditorTime : MonoBehaviour
{
    [ContextMenu("Generate New Shadow Object")]
    public void GenerateNewShadowObject()
    {
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        go.transform.parent = transform;
        go.name = "Shadow";

        StaticSpriteShadow shadow = go.AddComponent<StaticSpriteShadow>();
        shadow.ParentRenderer = GetComponent<SpriteRenderer>();
        shadow.MyRenderer = go.AddComponent<SpriteRenderer>();
        EditorUtility.SetDirty(gameObject);
        EditorUtility.SetDirty(go);
    }
}
#endif