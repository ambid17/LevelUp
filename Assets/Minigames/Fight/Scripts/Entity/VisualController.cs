using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class VisualController : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material flashMaterial;
        [SerializeField] protected Color defaultColor;
        [SerializeField] protected Color flashColor;


        [NonSerialized]
        public SpriteRenderer SpriteRenderer;
        [NonSerialized]
        public Animator Animator;

        protected EventService EventService;
        protected Entity MyEntity;

        private float _flashTimer;
        private const float FlashTime = 0.1f;
        private bool _isFlashing;

        private void Awake()
        {
            MyEntity = GetComponent<Entity>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            EventService = GameManager.EventService;
        }


        protected virtual void Start()
        {
            
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
            SpriteRenderer.material = flashMaterial;
            SpriteRenderer.color = flashColor;
            _isFlashing = true;
        }

        private void TryStopDamageFx()
        {
            if (_isFlashing)
            {
                _flashTimer += Time.deltaTime;

                if (_flashTimer > FlashTime)
                {
                    SpriteRenderer.material = defaultMaterial;
                    SpriteRenderer.color = defaultColor;
                    _flashTimer = 0;
                    _isFlashing = false;
                }
            }
        }
    }
}