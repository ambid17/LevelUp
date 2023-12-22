using UnityEngine;
using System.IO;
using Minigames.Fight;
using UnityEditor;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

/// <summary>
/// Takes snapshot of gameobject in scene and saves to PNG
/// 
/// To setup: 
/// - open the "RoomSnapshotGenerator" scene
/// - drag in a prefab into "objectToSnapshot"
/// - click on the ellipsis for this component and click "Take snapshot"
/// </summary>
public class SnapshotCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private List<GameObject> roomPrefabsToSnapshot;

    private const int imageSize = 256;
    private string AssetsSubdirectory = Path.Combine("Textures", "MinimapSprites");


    [ContextMenu("Take snapshot")]
    private void SnapshotGameobject()
    {
        foreach(var roomPrefab in roomPrefabsToSnapshot)
        {
            var roomToSnapshot = Instantiate(roomPrefab);
            var objectCollider = roomToSnapshot.GetComponent<Collider2D>();
            SnapCameraToCollder(objectCollider);

            var snapshot = TakeSnapshot(imageSize, imageSize);

            SavePNG(snapshot, $"{roomToSnapshot.name}.png");

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            var filePath = $"Assets/Textures/MinimapSprites/{roomToSnapshot.name}.png";

            UpdateSpriteSettings(filePath, objectCollider);
            AddSpriteToPrefab(filePath, objectCollider, roomPrefab.GetInstanceID());

            DestroyImmediate(roomToSnapshot);
        }
    }

    private void SnapCameraToCollder(Collider2D objectCollider)
    {
        var extents = objectCollider.bounds.extents;
        cam.orthographicSize = extents.x > extents.y ? extents.x : extents.y;

        var position = objectCollider.bounds.center;
        position.z = -10;
        cam.transform.position = position;
    }

    private void UpdateSpriteSettings(string filePath, Collider2D objectCollider)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;
        textureImporter.spritePixelsPerUnit = imageSize / objectCollider.bounds.size.x;

        EditorUtility.SetDirty(textureImporter);
        textureImporter.SaveAndReimport();
    }

    private void AddSpriteToPrefab(string filePath, Collider2D objectCollider, int instanceID)
    {
        var sprite = (Sprite)AssetDatabase.LoadAssetAtPath(filePath, typeof(Sprite));

        string prefabPath = AssetDatabase.GetAssetPath(instanceID);
        GameObject roomPrefab = PrefabUtility.LoadPrefabContents(prefabPath);

        var minimapGraphic = roomPrefab.GetComponentInChildren<MinimapRoomRender>();
        minimapGraphic.spriteRenderer.sprite = sprite;
        minimapGraphic.transform.localScale = Vector3.one;
        minimapGraphic.transform.position = objectCollider.bounds.center;
        minimapGraphic.defaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
        minimapGraphic.activeColor = Color.white;

        PrefabUtility.SaveAsPrefabAsset(roomPrefab, prefabPath);
        PrefabUtility.UnloadPrefabContents(roomPrefab);

    }

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

    public string SavePNG(Texture2D tex, string filename = "")
    {
        string directory = Path.Combine(Application.dataPath, AssetsSubdirectory);
        string filepath = Path.Combine(directory, filename);

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(filepath, bytes);

        return filepath;
    }
}