using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom2DShadows : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mainSpriteRenderer;
    [SerializeField]
    private float alpha;
    [SerializeField]
    private int offset;

    private SpriteRenderer _shadowSpriteRenderer;

    
    private void Start()
    {
        
    }

    [ContextMenu("GenerateShadowSprite")]
    public void GenerateShadow()
    {
        _shadowSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        Texture2D mainTexture = mainSpriteRenderer.sprite.texture;
        Texture2D shadowTexture = new(mainTexture.width, mainTexture.height);
        Sprite shadowSprite = Sprite.Create(shadowTexture, mainSpriteRenderer.sprite.rect, mainSpriteRenderer.sprite.pivot);
        _shadowSpriteRenderer.sprite = shadowSprite;
        for (int y = 0; y < mainTexture.height; y++)
        {
            for (int x = 0; x < mainTexture.width; x++)
            {
                if (mainTexture.GetPixel(x, y).a == 0)
                {
                    continue;
                }
                shadowTexture.SetPixel(x + (y * offset), y, new Color(0, 0, 0, alpha));
            }
        }
        shadowTexture.Apply();
    }
}
