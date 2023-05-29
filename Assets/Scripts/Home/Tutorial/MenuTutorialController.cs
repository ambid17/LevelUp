using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTutorialController : MonoBehaviour
{
    
    void Start()
    {
        TutorialState state = Tutorial.GetState();

        if (state == TutorialState.None)
        {
            Tutorial.SetState(TutorialState.Complete);
        }
    }

    void Update()
    {
        
    }
}
