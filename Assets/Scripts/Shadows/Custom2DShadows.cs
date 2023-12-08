using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Custom2DShadows : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mainSpriteRenderer;
    [SerializeField]
    private float alpha;
    [SerializeField]
    private int offset;
    [SerializeField]
    private int amountToWiden;

    private SpriteRenderer _shadowSpriteRenderer;

    private List<Vector2> _renderedCoordinates = new();

    private Texture2D _mainTexture;
    private Texture2D _shadowTexture;
    private Sprite _mainSprite;

    private void Start()
    {
        
    }

    [ContextMenu("GenerateShadowObject")]
    public void GenerateShadowObject()
    {
        GameObject go = new("Shadow");
        _shadowSpriteRenderer = go.AddComponent<SpriteRenderer>();
        _shadowSpriteRenderer.sortingLayerID = mainSpriteRenderer.sortingLayerID;
        _shadowSpriteRenderer.sortingOrder = mainSpriteRenderer.sortingOrder -1;
        _mainTexture = mainSpriteRenderer.sprite.texture;
        _mainSprite = mainSpriteRenderer.sprite;

        for (int y = 0; y < _mainTexture.height; y++)
        {
            for (int x = 0; x < _mainTexture.width; x++)
            {
                if (_mainTexture.GetPixel(x, y).a == 0 || y < _mainSprite.pivot.y)
                {
                    continue;
                }
                _renderedCoordinates.Add(new Vector2(x, y));
            }
        }

        _shadowTexture = new(_mainTexture.width + amountToWiden, _mainTexture.height);
        Sprite shadowSprite = Sprite.Create(_shadowTexture, new Rect(0,0,_mainSprite.rect.width + amountToWiden, _mainSprite.rect.height), new Vector2(_mainSprite.pivot.x / _mainSprite.rect.width, _mainSprite.pivot.y / _mainSprite.rect.height), _mainSprite.pixelsPerUnit, uint.MinValue, SpriteMeshType.FullRect, _mainSprite.border);
        _shadowSpriteRenderer.sprite = shadowSprite;


        for (int y = 0; y < _shadowTexture.height; y++)
        {
            for (int x = 0; x < _shadowTexture.width; x++)
            {
                _shadowTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        go.transform.localScale = transform.localScale;
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;

        UpdateShadow();
    }

    [ContextMenu("UpdateShadow")]
    public void UpdateShadow()
    {
        int difference = _shadowTexture.width - _mainTexture.width;

        foreach (Vector2 vector in _renderedCoordinates)
        {
            int offsetX = (int)vector.x + (difference / 2);
            _shadowTexture.SetPixel(offsetX + (((int)vector.y - (int)_mainSprite.pivot.y) * offset), (int)vector.y, new Color(0, 0, 0, alpha));
        }
        _shadowTexture.Apply();
    }
}
