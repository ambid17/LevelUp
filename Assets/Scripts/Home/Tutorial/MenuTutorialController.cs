using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTutorialController : MonoBehaviour
{
    [SerializeField] private WeaponTutorial _weaponTutorial;
    
    void Start()
    {
        TutorialState state = Tutorial.GetState();

        if (state == TutorialState.None)
        {
            Tutorial.SetState(TutorialState.ChooseWeapon);
            StartCoroutine(_weaponTutorial.Run());
        }
    }

    void Update()
    {
        
    }
}
