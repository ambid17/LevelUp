using System;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private bool _shouldOrbit;
    private SpriteRenderer _spriteRenderer;

    private const float planetSize = 6;
    private float planetSpeed = 0.1f;

    private int _sceneIndex;
    [SerializeField] private int _sceneIndexOffset = 2;
    [SerializeField] private float _timer;

    public World world;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timer = Random.Range(0, 360f);
        planetSpeed = Random.Range(0.01f, 0.3f);
    }

    private void Update()
    {
        if (!_shouldOrbit) return;
        
        _timer += Time.deltaTime;

        float radius = (_sceneIndex * planetSize) + 2;
        float angle = _timer * planetSpeed;
        Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        transform.position = position;
    }

    public void SetForWorld(World world)
    {
        this.world = world;
        gameObject.name = world.Name;
        _spriteRenderer.sprite = world.WorldSprite;
        _sceneIndex = world.SkillingSceneIndex - _sceneIndexOffset;
    }
}
