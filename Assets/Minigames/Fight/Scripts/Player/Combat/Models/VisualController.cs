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

        public SpriteRenderer spriteRenderer;
        public Animator animator;

        private EventService _eventService;

        private float _flashTimer;
        private readonly float FlashTime = 0.1f;
        private bool _isFlashing;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            _eventService = GameManager.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);
            _eventService.Add<PlayerRevivedEvent>(Revive);
            _eventService.Add<PlayerDamagedEvent>(StartDamageFx);
        }

        void Update()
        {
            TryStopDamageFx();
        }

        private void Revive()
        {
            spriteRenderer.color = Color.white;
        }

        private void Die()
        {
            spriteRenderer.color = Color.black;
        }

        private void StartDamageFx()
        {
            spriteRenderer.material = _flashMaterial;
            spriteRenderer.color = Color.white;
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
                    spriteRenderer.color = Color.white;
                    _flashTimer = 0;
                    _isFlashing = false;
                }
            }
        }
    }
}