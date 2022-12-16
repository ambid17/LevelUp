using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightUI : MonoBehaviour
{
    public TMP_Text goldText;
    
    void Start()
    {
        GameManager.Instance.currencyDidUpdate.AddListener(SetGoldText);
    }

    void Update()
    {
        
    }

    private void SetGoldText(float newValue)
    {
        goldText.text = newValue.ToCurrencyString();
    }
}
