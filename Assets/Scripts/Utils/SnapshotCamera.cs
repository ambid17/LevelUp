using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Object rendering code based on Dave Carlile's "Create a GameObject Image Using Render Textures" post
// Link: http://crappycoding.com/2014/12/create-gameobject-image-using-render-textures/

/// <summary>
/// Takes snapshot images of prefabs and GameObject instances, and provides methods to save them as PNG files.
/// </summary>
public class SnapshotCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private int layerToAssign;
    [SerializeField] private string snapshotFileName;
    [SerializeField] private GameObject objectToSnapshot;
    [SerializeField] private int imageSize;

    public float cameraOrthoSize;

    [ContextMenu("Generate minimap sprite")]
    private void GenerateMinimapSprite()
    {
        if (cam == null)
        {
            cam = MakeSnapshotCamera();
        }

        SetLayersRecursively(objectToSnapshot);

        var snapshot = TakeSnapshot(imageSize, imageSize);

        SavePNG(snapshot, $"{snapshotFileName}.png", Path.Combine("Textures", "MinimapSprites"));
    }

    public Camera MakeSnapshotCamera()
    {
        GameObject snapshotCameraGO = new GameObject("Snapshot Camera");
        Camera cam = snapshotCameraGO.AddComponent<Camera>();

        cam.cullingMask = 1 << layerToAssign;
        cam.orthographic = true;
        cam.orthographicSize = 10;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.clear;
        cam.nearClipPlane = 0.1f;

        return cam;
    }

    private void SetLayersRecursively(GameObject gameObject)
    {
        foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
            transform.gameObject.layer = layerToAssign;
    }

    /// <summary>
    /// Takes a snapshot of whatever is in front of the camera and within the camera's culling mask and returns it as a Texture2D.
    /// </summary>
    private Texture2D TakeSnapshot(int width, int height)
    {
        // Get a temporary render texture and render the camera
        cam.targetTexture = RenderTexture.GetTemporary(width, height, 24);
        cam.Render();

        // Activate the temporary render texture
        RenderTexture.active = cam.targetTexture;

        // Extract the image into a new texture without mipmaps
        Texture2D texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        texture.Apply(false);

        // Return the texture
        return texture;
    }

    public static FileInfo SavePNG(Texture2D tex, string filename = "", string directory = "")
    {
        directory = Path.Combine(Application.dataPath, directory);
        string filepath = Path.Combine(directory, filename);

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(filepath, bytes);

        return new FileInfo(filepath);
    }
}