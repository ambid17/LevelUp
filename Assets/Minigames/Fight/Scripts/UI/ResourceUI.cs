using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public Image MyImage;

    [SerializeField]
    private TMP_Text _myText;

    private float _myValue;

    public void UpdateValue(float value)
    {
        _myValue += value;
        _myText.text = _myValue.ToCurrencyString();
    }
}
