using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState
{
    None,
    ChooseWeapon,
    Complete
}
public static class Tutorial
{
    public static TutorialState GetState()
    {
        return GameManager.ProgressSettings.TutorialState;
    }

    public static void SetState(TutorialState newState)
    {
        GameManager.ProgressSettings.TutorialState = newState;
    }
    
    public static void CompleteState(TutorialState currentState)
    {
        TutorialState current = GetState();

        if (current == currentState)
        {
            Debug.Log($"[Tutorial] completed: {currentState} next: {currentState + 1}");
            GameManager.ProgressSettings.TutorialState = currentState + 1;
        }
        else
        {
            Debug.LogError("[Tutorial] tried to complete a state you are not in");
        }
    }
}
