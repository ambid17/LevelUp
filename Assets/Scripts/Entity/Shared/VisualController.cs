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
            StartCoroutine(DamageAnimation());
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
        protected virtual IEnumerator DamageAnimation()
        {
            MyEntity.Stunned = true;
            MyEntity.animationController.OverrideAnimation(takeHitAnimation, 0);

            // Because Unity animations refresh on LateUpdate the timing for taking a hit will sometimes prevent this from being true on the first frame.
            while (MyEntity.animationController.CurrentAnimation != takeHitAnimation)
            {
                MyEntity.animationController.OverrideAnimation(takeHitAnimation, 0);
                yield return new WaitForSeconds(0);
            }

            // If another animation is buffered, it may play just before the current animation is finished leading to undesired stun time.
            // By ensuring it's the correct animation we guarentee that the stun will end as soon as a different animation plays.
            while (MyEntity.animationController.CurrentAnimation == takeHitAnimation && !MyEntity.animationController.IsAnimFinished)
            {
                yield return new WaitForSeconds(0);
            }
            MyEntity.Stunned = false;

            // Just in case another animation doesn't play right away we don't want to get locked into the stun animation for another loop.
            MyEntity.animationController.ResetAnimations();
        }
    }
}