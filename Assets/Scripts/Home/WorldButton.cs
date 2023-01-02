using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    private RectTransform _rectTransform;

    private const float planetSize = 100;
    private float planetSpeed = 0.1f;

    private int _sceneIndex;
    [SerializeField] private float _timer;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _timer = Random.Range(0, 360f);
        planetSpeed = Random.Range(0.01f, 0.3f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        Vector2 position = new Vector2(Mathf.Cos(_timer * planetSpeed), Mathf.Sin(_timer * planetSpeed)) * (_sceneIndex * planetSize);
        _rectTransform.anchoredPosition = position;
    }

    public void SetForWorld(Action callback, World world)
    {
        _button.onClick.AddListener(() => callback());
        _image.sprite = world.WorldSprite;
        _sceneIndex = world.SceneIndex;
    }
}
