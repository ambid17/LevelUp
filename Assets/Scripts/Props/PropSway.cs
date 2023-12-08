using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropSway : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float strength = 2f;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float decelRate = 2f;
    [SerializeField]
    private float triggerDistance = .5f;
    [SerializeField]
    private float timeBetweenTriggers = .5f;

    private int _center = Shader.PropertyToID("_Center");
    private int _speed = Shader.PropertyToID("_Speed");
    private int _strength = Shader.PropertyToID("_Strength");

    private float _minSpeed;
    private float _minStrength;

    private Material _myMat;

    private List<Transform> _enteredEntities = new();
    private List<Transform> _triggeredEntities = new();
    private float _triggerTimer;

    [ContextMenu("GetComponent")]
    public void GetComponent()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        _myMat = spriteRenderer.material;
        float centerX = spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.rect.width;
        float centerY = spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.rect.height;

        _minSpeed = _myMat.GetFloat(_speed);
        _minStrength = _myMat.GetFloat(_strength);

        _myMat.SetVector(_center, new Vector2(centerX, centerY));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PhysicsUtils.PlayerLayer || collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.ProjectileLayer)
        {
            if (!_enteredEntities.Contains(collision.transform))
            {
                _enteredEntities.Add(collision.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PhysicsUtils.PlayerLayer || collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.ProjectileLayer)
        {
            _enteredEntities.Remove(collision.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (Transform t in _enteredEntities.ToList())
        {
            if (Vector2.Distance(t.position, transform.position) <= triggerDistance)
            {
                _enteredEntities.Remove(t);
                _triggeredEntities.Add(t);
                Shake();
            }
        }
        foreach (Transform t in _triggeredEntities.ToList())
        {
            if (Vector2.Distance(t.position, transform.position) > triggerDistance)
            {
                _triggeredEntities.Remove(t);
                _enteredEntities.Add(t);
                Shake();
            }
        }
    }

    private void Update()
    {
        _triggerTimer += Time.deltaTime;
        float newSpeed = Mathf.Clamp(_myMat.GetFloat(_speed) - Time.deltaTime * decelRate * (speed/strength), _minSpeed, speed);
        float newStrength = Mathf.Clamp(_myMat.GetFloat(_strength) - Time.deltaTime * decelRate, _minStrength, strength);
        _myMat.SetFloat(_speed, newSpeed);
        _myMat.SetFloat(_strength, newStrength);
    }

    private void Shake()
    {
        if (_triggerTimer < timeBetweenTriggers)
        {
            return;
        }
        _triggerTimer = 0;
        _myMat.SetFloat(_speed, speed);
        _myMat.SetFloat(_strength, strength);
    }
}