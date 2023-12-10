using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEditor;
using UnityEngine;

namespace Minigames.Fight
{
    public enum InteractionType
    {
        None, Upgrade, Craft
    }
    public class PlayerEntity : Entity
    {
        public PlayerAnimationController AnimationController => _animationControllerOverride;
        public Camera PlayerCamera => playerCamera;
        public PlayerWeaponArmController WeaponArmController => weaponArmController;

        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private PlayerWeaponArmController weaponArmController;
        [SerializeField]
        private Resource resourcePrefab;
        [SerializeField]
        private float maxResourceSpawns;

        private PlayerAnimationController _animationControllerOverride;

        private bool _canRevive;
        
        public float CurrentHp
        {
            get => Stats.currentHp;
            set
            {
                float newHp = value;
                // if adding to player hp, clamp it to max
                newHp = Mathf.Clamp(newHp, 0, GameManager.SettingsManager.playerSettings.MaxHp);
                Stats.currentHp = newHp;

                float hpPercent = Stats.currentHp / GameManager.SettingsManager.playerSettings.MaxHp;
                eventService.Dispatch(new PlayerHpUpdatedEvent(hpPercent));
            }
        }

        public InteractionType currentInteractionType = InteractionType.None; 
        
        protected override void Setup()
        {
            base.Setup();
            Stats.currentHp = GameManager.SettingsManager.playerSettings.MaxHp;
            eventService.Add<OnHitEffectUnlockedEvent>(SetupOnHitEffects);
            eventService.Add<OnCanInteractEvent>(OnCanInteract);
            SetupOnHitEffects(); // go ahead and query the onHit effects that were populated on load
            _animationControllerOverride = animationController as PlayerAnimationController;
        }

        private void SetupOnHitEffects()
        {
            Stats.OnHitEffects = GameManager.SettingsManager.effectSettings.OnHitEffects.OrderBy(e => e.ExecutionOrder).ToList();
        }

        private void OnCanInteract(OnCanInteractEvent e)
        {
            currentInteractionType = e.InteractionType;
        }

        public override void TakeDamage(float damage)
        {

            CurrentHp -= damage;

            // Damage FX are confusing if a hit only applies status effects.
            if (damage > 0)
            {
                VisualController.StartDamageFx(damage);
            }

            if (IsDead)
            {
                Die();
                StartCoroutine(WaitForRevive());
            }
        }

        protected override void Update()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            base.Update();

            if (Input.GetKeyDown(KeyCode.E) && currentInteractionType != InteractionType.None)
            {
                Interact();
            }
        }

        private void Interact()
        {
            // TODO: play interact animation
            eventService.Dispatch(new PlayerInteractedEvent(currentInteractionType));
        }
        
        protected override void Die()
        {
            _animationControllerOverride.PlayDieAnimation();
            eventService.Dispatch<PlayerDiedEvent>();
            Stats.StatusEffects.Clear();
            StartCoroutine(DropResources());
        }

        IEnumerator DropResources()
        {
            _canRevive = false;
            ResourceTypeFloatDictionary localDictionary = GameManager.CurrencyManager.PhysicalResources;
            List<ResourceType> keys = new(localDictionary.Keys);
            int spawnCap = 10;
            int spawnNumber = 0;

            float maxPerResource = maxResourceSpawns / keys.Count;

            foreach (ResourceType resourceType in keys)
            {
                float thisResourceCount = localDictionary[resourceType] / GameManager.CurrencyManager.ResourceValue;
                float thisResourceFactor = 1;

                if (thisResourceCount > maxPerResource)
                {
                    thisResourceFactor = maxPerResource / thisResourceCount;
                    thisResourceCount *= thisResourceFactor;
                }

                for (int i = 0; i < thisResourceCount; i++)
                {
                    Resource newResource = Instantiate(resourcePrefab, transform.position, transform.rotation);
                    newResource.Setup(GameManager.UIManager.ResourceSpriteDictionary[resourceType], resourceType, GameManager.CurrencyManager.ResourceValue / thisResourceFactor);
                    spawnNumber++;
                    if (spawnNumber >= spawnCap)
                    {
                        spawnNumber = 0;
                        yield return null;
                    }
                }
                GameManager.CurrencyManager.ResetResource(resourceType);
            }
            _canRevive = true;
        }
        
        private IEnumerator WaitForRevive()
        {
            while (!_animationControllerOverride.IsAnimFinished)
            {
                yield return null;
            }

            yield return new WaitForSeconds(GameManager.SettingsManager.incomeSettings.DeathTimer);

            while (!_canRevive)
            {
                yield return null;
            }

            transform.position = GameManager.RoomManager.StartRoom.Tilemap.cellBounds.center;

            // Wait a bit before reviving to make sure no physics updates happen before you're in the start room.
            yield return new WaitForSeconds(0.1f);

            CurrentHp = GameManager.SettingsManager.playerSettings.MaxHp;
            eventService.Dispatch<PlayerRevivedEvent>();
            _animationControllerOverride.ResetAnimations();
            _animationControllerOverride.PlayRunAnimation();
        }
    }
}