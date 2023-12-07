using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class VisualController : MonoBehaviour
    {
        [SerializeField] protected Material defaultMaterial;
        [SerializeField] private Material flashMaterial;
        [SerializeField] protected Color defaultColor = Color.white;
        [SerializeField] protected Color flashColor = Color.white;
        [SerializeField] protected AnimationName takeHitAnimation;


        [NonSerialized]
        public SpriteRenderer SpriteRenderer;
        [NonSerialized]
        public Animator Animator;

        protected EventService EventService;
        protected Entity MyEntity;

        private float _flashTimer;
        private const float FlashTime = 0.1f;
        protected bool IsFlashing;

        private void Awake()
        {
            MyEntity = GetComponent<Entity>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            EventService = Platform.EventService;
        }


        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            TryStopDamageFx();
        }

        public virtual void StartDamageFx(float damage)
        {
            DamageAnimation();
            SpriteRenderer.material = flashMaterial;
            SpriteRenderer.color = flashColor;
            IsFlashing = true;
        }

        private void TryStopDamageFx()
        {
            if (IsFlashing)
            {
                _flashTimer += Time.deltaTime;

                if (_flashTimer > FlashTime)
                {
                    SpriteRenderer.material = defaultMaterial;
                    SpriteRenderer.color = defaultColor;
                    _flashTimer = 0;
                    IsFlashing = false;
                }
            }
        }
        protected virtual void DamageAnimation()
        {
            MyEntity.Stunned = true;
            StartCoroutine(MyEntity.animationController.Stun(takeHitAnimation, AfterStun));
            
        }
        private void AfterStun()
        {
            MyEntity.Stunned = false;
        }
    }
}