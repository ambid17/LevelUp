    using System;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private bool _shouldOrbit;
    [SerializeField] private int _lineSegments;
    private SpriteRenderer _spriteRenderer;
    private LineRenderer _lineRenderer;

    private const float planetSize = 6;
    private float planetSpeed = 0.1f;

    private int _sceneIndex;
    [SerializeField] private int _sceneIndexOffset = 2;
    [SerializeField] private float _timer;

    public Biome world;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timer = Random.Range(0, 360f);
        planetSpeed = Random.Range(0.01f, 0.3f);
    }

    private void Update()
    {
        if (!_shouldOrbit) return;
        
        _timer += Time.deltaTime;

        float angle = _timer * planetSpeed;
        transform.position = GetRadialPosition(angle);
    }

    public void SetForWorld(Biome world)
    {
        this.world = world;
        gameObject.name = world.Name;
        _spriteRenderer.sprite = world.BiomeSprite;
        _sceneIndex = SceneManager.GetSceneByName("Fight").buildIndex;
        SetOrbitLine();
    }

    private void SetOrbitLine()
    {
        var pointCount = _lineSegments + 1; // Extra point to close the circle
        _lineRenderer.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * 360f / _lineSegments);
            Vector3 position = GetRadialPosition(angle);
            _lineRenderer.SetPosition(i, position);
        }
    }

    private Vector3 GetRadialPosition(float angle)
    {
        float radius = (_sceneIndex * planetSize) + 2;
        Vector2 position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return new Vector3(position.x, position.y, 0);
    }
}
