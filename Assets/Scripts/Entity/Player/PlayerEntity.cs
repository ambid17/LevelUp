using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using Minigames.Fight;
using Pathfinding;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Minigames.Fight
{
    public enum InteractionType
    {
        None, Upgrade, Craft
    }

    public enum PlayerControlledActionType
    {
        Craft,
        BossRoomEntry,
    }

    public class PlayerEntity : Entity
    {
        public PlayerAnimationController AnimationController => _animationControllerOverride;

        [SerializeField]
        private Resource resourcePrefab;
        [SerializeField]
        private float maxResourceSpawns;

        private PlayerAnimationController _animationControllerOverride;

        private bool _hasFinishedDroppingResources;

        public InteractionType currentInteractionType = InteractionType.None;

        protected override void Setup()
        {
            base.Setup();
            eventService.Add<OnCanInteractEvent>(OnCanInteract);
            _animationControllerOverride = base.AnimationController as PlayerAnimationController;
        }


        private void OnCanInteract(OnCanInteractEvent e)
        {
            currentInteractionType = e.InteractionType;
        }

        public override void TakeHit(float damage, Entity hitter)
        {
            base.TakeHit(damage, hitter);
            eventService.Dispatch<PlayerHpUpdatedEvent>();
        }

        protected override void Update()
        {
            if (IsDead)
            {
                return;
            }
            base.Update();

            // Let statuses still tick, but don't allow overriding interaction
            if (IsControlled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && currentInteractionType != InteractionType.None)
            {
                Interact();
            }
        }

        private void Interact()
        {
            // TODO: play interact animation
            if(currentInteractionType == InteractionType.Craft)
            {
                PlayerPathfindingMovementController pathfindingMovementController = gameObject.AddComponent<PlayerPathfindingMovementController>();
                pathfindingMovementController.StartPath(GameManager.RoomManager.BossRoom.ConstructionChamber.PlayerMoveTarget, PlayerControlledActionType.Craft);
            }
        }

        protected override void Die(Entity killer)
        {
            base.Die(killer);
            _animationControllerOverride.PlayDieAnimation();
            eventService.Dispatch<PlayerDiedEvent>();
            Stats.ClearAllStatusEffects();
            StartCoroutine(DropResources());
            StartCoroutine(WaitForRevive());
        }

        IEnumerator DropResources()
        {
            _hasFinishedDroppingResources = false;
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
            }
            GameManager.CurrencyManager.PhysicalResources = new();
            _hasFinishedDroppingResources = true;
        }

        private IEnumerator WaitForRevive()
        {
            while (!_animationControllerOverride.IsAnimFinished)
            {
                yield return new WaitForSeconds(0);
            }

            while (!_hasFinishedDroppingResources)
            {
                yield return new WaitForSeconds(0);
            }

            transform.position = GameManager.RoomManager.StartRoom.Tilemap.cellBounds.center;

            // TODO: play revive sequence with the robot butler
            // Wait a bit before reviving to make sure no physics updates happen before you're in the start room.
            yield return new WaitForSeconds(0.1f);

            eventService.Dispatch<PlayerRevivedEvent>();
            _animationControllerOverride.ResetAnimations();
        }
    }
}