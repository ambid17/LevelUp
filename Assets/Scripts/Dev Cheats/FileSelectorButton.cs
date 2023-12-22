using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileSelectorButton : MonoBehaviour
{
    public Button button;
    [SerializeField] private Image buttonImage;
    public TMP_Text buttonText;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    public void Select()
    {
        buttonImage.sprite = selectedSprite;
    }

    public void Deselect()
    {
        buttonImage.sprite = defaultSprite;
    }
}
