using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowData", menuName = "ScriptableObjects/ShadowData", order = 1)]
[Serializable]
public class ShadowData : ScriptableObject
{
    public SpriteShadowSpritedataDictionary SpriteShadowMappings;

    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private List<Sprite> spritesToOverride;
    [SerializeField]
    private Color shadowColor;
    [SerializeField]
    private int shadowOffset;

    [ContextMenu("Generate Shadow Sprites")]
    public void GenerateShadows()
    {
        sprites = new();
        SpriteShadowMappings = new();

        // Guids for all assets inside folder for sprites with shadows.
        string[] assetGuids = AssetDatabase.FindAssets("t:sprite", new[] {"Assets/Textures/HasShadows" });

        // Generate a list of sprites in these asset paths.
        foreach (string guid in assetGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
            sprites.Add(sprite);
        }

        // Generate the dictionary
        foreach (Sprite sprite in sprites)
        {
            List<Vector2> renderedCoordinates = new();
            Texture2D mainTexture = sprite.texture;

            // Go through all coordinates in the texture and add every coordinate that has a color and is above the pivot point.
            for (int y = 0; y < mainTexture.height; y++)
            {
                for (int x = 0; x < mainTexture.width; x++)
                {
                    if (mainTexture.GetPixel(x, y).a == 0 || y < sprite.pivot.y)
                    {
                        continue;
                    }
                    renderedCoordinates.Add(new Vector2(x, y));
                }
            }

            // Generates new sprites based on the initial sprite.
            Texture2D unflippedShadowTexture = new(mainTexture.width * 4, mainTexture.height);
            Texture2D flippedShadowTexture = new(mainTexture.width * 4, mainTexture.height);

            // Calculate the width difference between the two textures to offset the sprite positions.
            int flippedDifference = flippedShadowTexture.width - mainTexture.width;
            int unflippedDifference = unflippedShadowTexture.width - mainTexture.width;

            // Unity defaults it's texures to white so we have to turn them all transparent.
            for (int y = 0; y < unflippedShadowTexture.height; y++)
            {
                for (int x = 0; x < unflippedShadowTexture.width; x++)
                {
                    unflippedShadowTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            for (int y = 0; y < flippedShadowTexture.height; y++)
            {
                for (int x = 0; x < flippedShadowTexture.width; x++)
                {
                    flippedShadowTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }

            // Render each pixel from the original texture as the shadow color with an offset on the X axis
            foreach (Vector2 vector in renderedCoordinates)
            {
                Color color = shadowColor;
                // Reduce the alpha of the pixel based on the alpha of the sprite.
                color.a -= Mathf.Clamp((1 - mainTexture.GetPixel((int)vector.x, (int)vector.y).a), 0, 1);

                int offsetX = (int)vector.x + (unflippedDifference / 2) - ((int)sprite.rect.center.x - (int)sprite.pivot.x);
                unflippedShadowTexture.SetPixel(offsetX + (((int)vector.y - (int)sprite.pivot.y) * shadowOffset), (int)vector.y, color);
            }
            foreach (Vector2 vector in renderedCoordinates)
            {
                Color color = shadowColor;
                // Reduce the alpha of the pixel based on the alpha of the sprite.
                color.a -= Mathf.Clamp((1 - mainTexture.GetPixel((int)vector.x, (int)vector.y).a), 0, 1);

                int offsetX = (int)vector.x + (flippedDifference / 2) - ((int)sprite.rect.center.x - (int)sprite.pivot.x);
                flippedShadowTexture.SetPixel(offsetX + (((int)vector.y - (int)sprite.pivot.y) * -shadowOffset), (int)vector.y, color);
            }
            unflippedShadowTexture.Apply();
            flippedShadowTexture.Apply();

            string unflippedPath = "Assets/Textures/ShadowSprites/" + mainTexture.name + "_Shadow.png";
            string flippedPath = "Assets/Textures/ShadowSprites/" + mainTexture.name + "_Flipped_Shadow.png";

            Sprite unflippedShadowSprite = Sprite.Create(unflippedShadowTexture, new Rect(0, 0, sprite.rect.width * 4, sprite.rect.height), new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height), sprite.pixelsPerUnit, uint.MinValue, SpriteMeshType.FullRect, sprite.border);
            Sprite flippedShadowSprite = Sprite.Create(flippedShadowTexture, new Rect(0, 0, sprite.rect.width * 4, sprite.rect.height), new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height), sprite.pixelsPerUnit, uint.MinValue, SpriteMeshType.FullRect, sprite.border);

            unflippedShadowSprite.name = sprite.name + "_Shadow";
            flippedShadowSprite.name = sprite.name + "_Flipped_Shadow";

            File.WriteAllBytes(unflippedPath, unflippedShadowTexture.EncodeToPNG());
            File.WriteAllBytes(flippedPath, flippedShadowTexture.EncodeToPNG());

            AssetDatabase.Refresh();

            AssetDatabase.AddObjectToAsset(unflippedShadowSprite, unflippedPath);
            AssetDatabase.AddObjectToAsset(flippedShadowSprite, flippedPath);

            AssetDatabase.SaveAssetIfDirty(new GUID(AssetDatabase.AssetPathToGUID(unflippedPath)));
            AssetDatabase.SaveAssetIfDirty(new GUID(AssetDatabase.AssetPathToGUID(flippedPath)));


            TextureImporter unflippedTextureImporter = AssetImporter.GetAtPath(unflippedPath) as TextureImporter;

            TextureImporterSettings unflippedTexSettings = new TextureImporterSettings();

            unflippedTextureImporter.ReadTextureSettings(unflippedTexSettings);
            unflippedTexSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            unflippedTexSettings.spriteMeshType = SpriteMeshType.FullRect;
            unflippedTextureImporter.SetTextureSettings(unflippedTexSettings);
            unflippedTextureImporter.spritePixelsPerUnit = unflippedShadowSprite.pixelsPerUnit;
            unflippedTextureImporter.spritePivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            unflippedTextureImporter.spriteBorder = unflippedShadowSprite.border;

            EditorUtility.SetDirty(unflippedTextureImporter);

            TextureImporter flippedTextureImporter = AssetImporter.GetAtPath(flippedPath) as TextureImporter;

            TextureImporterSettings flippedTexSettings = new TextureImporterSettings();

            flippedTextureImporter.ReadTextureSettings(flippedTexSettings);
            flippedTexSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            flippedTexSettings.spriteMeshType = SpriteMeshType.Tight;
            flippedTextureImporter.SetTextureSettings(flippedTexSettings);

            flippedTextureImporter.spritePixelsPerUnit = flippedShadowSprite.pixelsPerUnit;
            flippedTextureImporter.spritePivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            flippedTextureImporter.spriteBorder = flippedShadowSprite.border;
            EditorUtility.SetDirty(flippedTextureImporter);
            unflippedTextureImporter.SaveAndReimport();
            flippedTextureImporter.SaveAndReimport();

            Sprite finalUnflippedSprite = (Sprite)AssetDatabase.LoadAssetAtPath(unflippedPath, typeof(Sprite));
            Sprite finalFlippedSprite = (Sprite)AssetDatabase.LoadAssetAtPath(flippedPath, typeof(Sprite));

            SpriteShadowMappings.Add(sprite, new(finalFlippedSprite, finalUnflippedSprite));
        }
        Debug.Log("Shadow Sprites Succesfully Generated");
    }

    [ContextMenu("OverwriteShadows")]
    public void OverwriteShadows()
    {
        // Generate the dictionary
        foreach (Sprite sprite in spritesToOverride)
        {
            List<Vector2> renderedCoordinates = new();
            Texture2D mainTexture = sprite.texture;

            // Go through all coordinates in the texture and add every coordinate that has a color and is above the pivot point.
            for (int y = 0; y < mainTexture.height; y++)
            {
                for (int x = 0; x < mainTexture.width; x++)
                {
                    if (mainTexture.GetPixel(x, y).a == 0 || y < sprite.pivot.y)
                    {
                        continue;
                    }
                    renderedCoordinates.Add(new Vector2(x, y));
                }
            }

            // Generates new sprites based on the initial sprite.
            Texture2D unflippedShadowTexture = new(mainTexture.width * 4, mainTexture.height);
            Texture2D flippedShadowTexture = new(mainTexture.width * 4, mainTexture.height);

            // Calculate the width difference between the two textures to offset the sprite positions.
            int flippedDifference = flippedShadowTexture.width - mainTexture.width;
            int unflippedDifference = unflippedShadowTexture.width - mainTexture.width;

            // Unity defaults it's texures to white so we have to turn them all transparent.
            for (int y = 0; y < unflippedShadowTexture.height; y++)
            {
                for (int x = 0; x < unflippedShadowTexture.width; x++)
                {
                    unflippedShadowTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            for (int y = 0; y < flippedShadowTexture.height; y++)
            {
                for (int x = 0; x < flippedShadowTexture.width; x++)
                {
                    flippedShadowTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }

            // Render each pixel from the original texture as the shadow color with an offset on the X axis
            foreach (Vector2 vector in renderedCoordinates)
            {
                Color color = shadowColor;
                // Reduce the alpha of the pixel based on the alpha of the sprite.
                color.a -= Mathf.Clamp((1 - mainTexture.GetPixel((int)vector.x, (int)vector.y).a), 0, 1);

                int offsetX = (int)vector.x + (unflippedDifference / 2) - ((int)sprite.rect.center.x - (int)sprite.pivot.x);
                unflippedShadowTexture.SetPixel(offsetX + (((int)vector.y - (int)sprite.pivot.y) * shadowOffset), (int)vector.y, color);
            }
            foreach (Vector2 vector in renderedCoordinates)
            {
                Color color = shadowColor;
                // Reduce the alpha of the pixel based on the alpha of the sprite.
                color.a -= Mathf.Clamp((1 - mainTexture.GetPixel((int)vector.x, (int)vector.y).a), 0, 1);

                int offsetX = (int)vector.x + (flippedDifference / 2) - ((int)sprite.rect.center.x - (int)sprite.pivot.x);
                flippedShadowTexture.SetPixel(offsetX + (((int)vector.y - (int)sprite.pivot.y) * -shadowOffset), (int)vector.y, color);
            }
            unflippedShadowTexture.Apply();
            flippedShadowTexture.Apply();

            string unflippedPath = "Assets/Textures/ShadowSprites/" + mainTexture.name + "_Shadow.png";
            string flippedPath = "Assets/Textures/ShadowSprites/" + mainTexture.name + "_Flipped_Shadow.png";

            Sprite unflippedShadowSprite = Sprite.Create(unflippedShadowTexture, new Rect(0, 0, sprite.rect.width * 4, sprite.rect.height), new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height), sprite.pixelsPerUnit, uint.MinValue, SpriteMeshType.FullRect, sprite.border);
            Sprite flippedShadowSprite = Sprite.Create(flippedShadowTexture, new Rect(0, 0, sprite.rect.width * 4, sprite.rect.height), new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height), sprite.pixelsPerUnit, uint.MinValue, SpriteMeshType.FullRect, sprite.border);

            unflippedShadowSprite.name = sprite.name + "_Shadow";
            flippedShadowSprite.name = sprite.name + "_Flipped_Shadow";

            File.WriteAllBytes(unflippedPath, unflippedShadowTexture.EncodeToPNG());
            File.WriteAllBytes(flippedPath, flippedShadowTexture.EncodeToPNG());

            AssetDatabase.Refresh();

            AssetDatabase.AddObjectToAsset(unflippedShadowSprite, unflippedPath);
            AssetDatabase.AddObjectToAsset(flippedShadowSprite, flippedPath);

            AssetDatabase.SaveAssetIfDirty(new GUID(AssetDatabase.AssetPathToGUID(unflippedPath)));
            AssetDatabase.SaveAssetIfDirty(new GUID(AssetDatabase.AssetPathToGUID(flippedPath)));


            TextureImporter unflippedTextureImporter = AssetImporter.GetAtPath(unflippedPath) as TextureImporter;

            TextureImporterSettings unflippedTexSettings = new TextureImporterSettings();

            unflippedTextureImporter.ReadTextureSettings(unflippedTexSettings);
            unflippedTexSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            unflippedTexSettings.spriteMeshType = SpriteMeshType.FullRect;
            unflippedTextureImporter.SetTextureSettings(unflippedTexSettings);
            unflippedTextureImporter.spritePixelsPerUnit = unflippedShadowSprite.pixelsPerUnit;
            unflippedTextureImporter.spritePivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            unflippedTextureImporter.spriteBorder = unflippedShadowSprite.border;

            EditorUtility.SetDirty(unflippedTextureImporter);

            TextureImporter flippedTextureImporter = AssetImporter.GetAtPath(flippedPath) as TextureImporter;

            TextureImporterSettings flippedTexSettings = new TextureImporterSettings();

            flippedTextureImporter.ReadTextureSettings(flippedTexSettings);
            flippedTexSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            flippedTexSettings.spriteMeshType = SpriteMeshType.FullRect;
            flippedTextureImporter.SetTextureSettings(flippedTexSettings);

            flippedTextureImporter.spritePixelsPerUnit = flippedShadowSprite.pixelsPerUnit;
            flippedTextureImporter.spritePivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            flippedTextureImporter.spriteBorder = flippedShadowSprite.border;
            EditorUtility.SetDirty(flippedTextureImporter);
            unflippedTextureImporter.SaveAndReimport();
            flippedTextureImporter.SaveAndReimport();

            Sprite finalUnflippedSprite = (Sprite)AssetDatabase.LoadAssetAtPath(unflippedPath, typeof(Sprite));
            Sprite finalFlippedSprite = (Sprite)AssetDatabase.LoadAssetAtPath(flippedPath, typeof(Sprite));

            SpriteShadowMappings[sprite] = new(finalFlippedSprite, finalUnflippedSprite);
        }
    }
}

[Serializable]
public class ShadowSpriteData
{
    public Sprite ShadowSprite(bool flipped) => flipped == true ? _flipped : _unflipped;

    [SerializeField]
    private Sprite _flipped;
    [SerializeField]
    private Sprite _unflipped;

    public ShadowSpriteData(Sprite flipped, Sprite unflipped)
    {
        _flipped = flipped;
        _unflipped = unflipped;
    }
}