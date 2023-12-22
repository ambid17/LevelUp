using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropSway : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] private SpriteRenderer shadowRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float strength = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float decelRate = 2f;
    [SerializeField] private float timeBetweenTriggers = .5f;
    
    [Header("Used for debugging, set at runtime")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _minStrength;
    [SerializeField] private float _triggerTimer;

    private Material _myMat;
    private Material _shadowMat;

    private int _speed = Shader.PropertyToID("_Speed");
    private int _strength = Shader.PropertyToID("_Strength");

    private void Awake()
    {
        _myMat = spriteRenderer.material;
        if (shadowRenderer != null)
        {
            _shadowMat = shadowRenderer.material;
        }

        _minSpeed = _myMat.GetFloat(_speed);
        _minStrength = _myMat.GetFloat(_strength);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PhysicsUtils.PlayerLayer || collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.ProjectileLayer)
        {
            Shake();
        }
    }

    private void Update()
    {
        _triggerTimer += Time.deltaTime;

        float newSpeed = _myMat.GetFloat(_speed) - Time.deltaTime * decelRate * (speed / strength);
        newSpeed = Mathf.Clamp(newSpeed, _minSpeed, speed);

        float newStrength = _myMat.GetFloat(_strength) - Time.deltaTime * decelRate;
        newStrength = Mathf.Clamp(_myMat.GetFloat(_strength) - Time.deltaTime * decelRate, _minStrength, strength);

        _myMat.SetFloat(_speed, newSpeed);
        _myMat.SetFloat(_strength, newStrength);
        if (_shadowMat != null)
        {
            _shadowMat.SetFloat(_speed, newSpeed);
            _shadowMat.SetFloat(_strength, newStrength);
        }
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
        if (_shadowMat != null)
        {
            _shadowMat.SetFloat(_speed, speed);
            _shadowMat.SetFloat(_strength, strength);
        }
    }

    [ContextMenu("GetComponent")]
    public void GetComponent()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}