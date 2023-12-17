using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RoomSnapshotGenerator : MonoBehaviour
{
    public GameObject prefabToSnapshot;

    public float cameraOrthoSize;

    [ContextMenu("Generate minimap sprite")]
    private void GenerateMinimapSprite()
    {
        // Create a new GameObject to hold the camera
        SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(cameraOrthoSize,31);
        var snapshot = snapshotCamera.TakePrefabSnapshot(prefabToSnapshot);
        SnapshotCamera.SavePNG(snapshot, $"{prefabToSnapshot.name}1.png", Path.Combine("Textures", "MinimapSprites"));

        //DestroyImmediate(snapshotCamera.gameObject);
    }
}
