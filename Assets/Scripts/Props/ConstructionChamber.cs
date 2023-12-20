using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ConstructionChamber : MonoBehaviour
{
    [SerializeField] private AnimationName idle;
    [SerializeField] private AnimationName doorOpen;
    [SerializeField] private AnimationName doorClose;
    [SerializeField] private AnimationName finishedUpgrade;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private Transform playerMoveTarget;
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
    }

    private void OnCanInteract(OnCanInteractEvent e)
    {
        if(e.InteractionType == InteractionType.Craft)
        {
            animationManager.PlayAnimation(doorOpen);
            isDoorOpen = true;
        }
        else if(isDoorOpen)
        {
            animationManager.PlayAnimation(doorClose);
            isDoorOpen = false;
        }
    }

    private void OnPlayerInteracted(PlayerControlledActionFinishedEvent e)
    {
        if(e.InteractionType == InteractionType.Craft)
        {
            StartCoroutine(animationManager.PlayAnimationWithCallback(doorClose, OnStartCraft));
        }
    }

    private void OnStartCraft()
    {
        eventService.Dispatch(new PlayerInteractedEvent(InteractionType.Craft));
    }

    private void OnCraftUpgrade()
    {
        didCraftUpgrade = true;
    }

    private void OnFinishCraft()
    {
        if(didCraftUpgrade)
        {
            StartCoroutine(animationManager.PlayAnimationWithCallback(finishedUpgrade, () => animationManager.PlayAnimation(doorOpen)));
        }

        didCraftUpgrade = false;
    }
}
