using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public SpriteRenderer MySpriteRenderer;

    [SerializeField]
    private TMP_Text _myText;

    private float _myValue;

    public void UpdateValue(float value)
    {
        _myValue += value;
        _myText.text = _myValue.ToString();
    }
}
