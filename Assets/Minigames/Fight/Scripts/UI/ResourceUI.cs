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

    public void UpdateValue(float value)
    {
        _myText.text = value.ToCurrencyString();
    }
}
