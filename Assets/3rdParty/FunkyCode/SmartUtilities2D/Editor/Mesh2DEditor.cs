﻿using UnityEditor;
using UnityEngine;

namespace FunkyCode.Utilities
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Mesh2D))]
	public class Mesh2DEditor : UnityEditor.Editor {

		override public void OnInspectorGUI() {
			Mesh2D script = target as Mesh2D;

			script.triangulation = (PolygonTriangulator2D.Triangulation)EditorGUILayout.EnumPopup("Triangulation Type", script.triangulation);
			
			script.material = (Material)EditorGUILayout.ObjectField(script.material, typeof(Material), true);

			script.materialScale = EditorGUILayout.Vector2Field("Material Scale", script.materialScale);
			script.materialOffset = EditorGUILayout.Vector2Field("Material Offset", script.materialOffset);

			script.sortingLayerName = EditorGUILayout.TextField("Sorting Layer Name", script.sortingLayerName);
			script.sortingOrder = EditorGUILayout.IntField("Sorting Order", script.sortingOrder);
			
			if (GUILayout.Button("Update Mesh")) {
				script.Initialize();
			}
		}
	}
}