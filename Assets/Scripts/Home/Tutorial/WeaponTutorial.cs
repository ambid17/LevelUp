using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTutorial : MonoBehaviour
{
    [SerializeField] private GameObject weaponSelectContainer;
    [SerializeField] private Button weaponSelectButton;
    
    public IEnumerator Run()
    {
        weaponSelectContainer.SetActive(true);

        yield return new WaitForButtonClick(weaponSelectButton);
        
        weaponSelectContainer.SetActive(false);
        
        Tutorial.CompleteState(TutorialState.ChooseWeapon);
    }
}
