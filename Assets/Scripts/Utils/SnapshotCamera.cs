using UnityEngine;
using System.IO;

/// <summary>
/// Takes snapshot of gameobject in scene and saves to PNG
/// 
/// To setup: 
/// - open the "RoomSnapshotGenerator" scene
/// - drag in a prefab to the scene that you want to screenshot
/// - drag the reference of the instantiated prefab into "objectToSnapshot"
/// - click on the ellipsis for this component and click "Take snapshot"
/// </summary>
public class SnapshotCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject objectToSnapshot;
    private const int imageSize = 256;


    [ContextMenu("Take snapshot")]
    private void SnapshotGameobject()
    {
        var objectCollider = objectToSnapshot.GetComponent<Collider2D>();

        var extents = objectCollider.bounds.extents;
        cam.orthographicSize = extents.x > extents.y ? extents.x : extents.y;

        var position = objectCollider.bounds.center;
        position.z = -10;
        cam.transform.position = position;

        var snapshot = TakeSnapshot(imageSize, imageSize);

        SavePNG(snapshot, $"{objectToSnapshot.name}.png", Path.Combine("Textures", "MinimapSprites"));
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

    public static FileInfo SavePNG(Texture2D tex, string filename = "", string directory = "")
    {
        directory = Path.Combine(Application.dataPath, directory);
        string filepath = Path.Combine(directory, filename);

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(filepath, bytes);

        return new FileInfo(filepath);
    }
}