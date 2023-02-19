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


        public SpriteRenderer spriteRenderer;
        public Animator animator;

        protected EventService eventService;

        private float _flashTimer;
        private readonly float FlashTime = 0.1f;
        private bool _isFlashing;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            eventService = GameManager.EventService;
        }

        protected virtual void Update()
        {
            TryStopDamageFx();
        }

        public virtual void StartDamageFx(float damage)
        {
            spriteRenderer.material = _flashMaterial;
            spriteRenderer.color = _flashColor;
            _isFlashing = true;
        }

        private void TryStopDamageFx()
        {
            if (_isFlashing)
            {
                _flashTimer += Time.deltaTime;

                if (_flashTimer > FlashTime)
                {
                    spriteRenderer.material = _defaultMaterial;
                    spriteRenderer.color = _defaultColor;
                    _flashTimer = 0;
                    _isFlashing = false;
                }
            }
        }
    }
}