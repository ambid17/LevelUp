using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    public TMP_Text goldText;
    public Slider hpSlider;
    
    void Start()
    {
        GameManager.Instance.currencyDidUpdate.AddListener(SetGoldText);
        GameManager.Instance.hpDidUpdate.AddListener(SetHpSlider);
    }

    void Update()
    {
        
    }

    private void SetGoldText(float newValue)
    {
        goldText.text = newValue.ToCurrencyString();
    }

    private void SetHpSlider(float hpPercentage)
    {
        hpSlider.value = hpPercentage;
    }
}
