using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class VisualController : MonoBehaviour
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected Color _flashColor;


        [NonSerialized]
        public SpriteRenderer SpriteRenderer;
        [NonSerialized]
        public Animator Animator;

        protected EventService EventService;
        protected Entity MyEntity;

        private float _flashTimer;
        private const float FlashTime = 0.1f;
        private bool _isFlashing;


        protected virtual void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();

            EventService = GameManager.EventService;
        }

        protected virtual void Update()
        {
            FlipSprite();
            TryStopDamageFx();
        }
        
        protected void  FlipSprite()
        {
            //Flip the sprite based on velocity
            if(MyEntity.MovementController.MyRigidbody2D.velocity.x < 0) 
                SpriteRenderer.flipX = true;
            else 
                SpriteRenderer.flipX = false;
        }

        public virtual void StartDamageFx(float damage)
        {
            SpriteRenderer.material = _flashMaterial;
            SpriteRenderer.color = _flashColor;
            _isFlashing = true;
        }

        private void TryStopDamageFx()
        {
            if (_isFlashing)
            {
                _flashTimer += Time.deltaTime;

                if (_flashTimer > FlashTime)
                {
                    SpriteRenderer.material = _defaultMaterial;
                    SpriteRenderer.color = _defaultColor;
                    _flashTimer = 0;
                    _isFlashing = false;
                }
            }
        }
    }
}