using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class ConstructionChamber : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer => spriteRenderer;

        [SerializeField] private AnimationName idle;
        [SerializeField] private AnimationName doorOpen;
        [SerializeField] private AnimationName doorClose;
        [SerializeField] private AnimationName finishedUpgrade;
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private Transform playerMoveTarget;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public Vector3 PlayerMoveTarget => playerMoveTarget.position;

        private EventService eventService;
        private bool isDoorOpen;
        private bool didCraftUpgrade;
        // Player walks up, door opens up
        // player clicks interact, pathfinds to chamber
        // door closes, UI pops up
        // once done upgrading, run spit out animation
        // door opens
        void Start()
        {
            eventService = Platform.EventService;
            eventService.Add<OnCanInteractEvent>(OnCanInteract);
            eventService.Add<PlayerControlledActionFinishedEvent>(OnPlayerInteracted);
            eventService.Add<DidCraftUpgradeEvent>(OnCraftUpgrade);
            eventService.Add<ClosedCraftingUiEvent>(OnFinishCraft);
        }

        private void OnCanInteract(OnCanInteractEvent e)
        {
            if (e.InteractionType == InteractionType.Craft)
            {
                animationManager.PlayAnimation(doorOpen);
                isDoorOpen = true;
            }
            else if (isDoorOpen)
            {
                animationManager.PlayAnimation(doorClose);
                isDoorOpen = false;
            }
        }

        private void OnPlayerInteracted(PlayerControlledActionFinishedEvent e)
        {
            if (e.ActionType == PlayerControlledActionType.Craft)
            {
                StartCoroutine(animationManager.PlayAnimationWithCallback(doorClose, () =>
                {
                    eventService.Dispatch(new PlayerInteractedEvent(InteractionType.Craft));
                }));
            }
        }

        private void OnCraftUpgrade()
        {
            didCraftUpgrade = true;
        }

        private void OnFinishCraft()
        {
            if (didCraftUpgrade)
            {
                StartCoroutine(animationManager.PlayAnimationWithCallback(finishedUpgrade, () => OpenDoor()));
            }
            else
            {
                OpenDoor();
            }

            didCraftUpgrade = false;
        }

        private void OpenDoor()
        {
            StartCoroutine(animationManager.PlayAnimationWithCallback(doorOpen, () => GameManager.PlayerEntity.IsControlled = false));
        }
    }
}