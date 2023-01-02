using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    private RectTransform _rectTransform;

    private const float planetSize = 100;
    private const float planetSpeed = 0.1f;

    private World _world;
    private int _sceneIndex;
    [SerializeField] private float _timer;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _timer = Random.Range(0, 360f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        Vector2 position = new Vector2(Mathf.Cos(_timer * planetSpeed), Mathf.Sin(_timer * planetSpeed)) * _sceneIndex * planetSize;
        _rectTransform.anchoredPosition = position;
    }

    public void SetForWorld(Action callback, World world)
    {
        _world = world;
        _button.onClick.AddListener(() => callback());
        _image.sprite = world.WorldSprite;
        _text.text = world.Name;
        _sceneIndex = world.SceneIndex;
    }
}
