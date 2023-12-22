using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatsRow : MonoBehaviour
{
    public TMP_Text entityNameText;
    public TMP_Text entityFileNameText;
    public Button rowButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    public void Setup(string entityName, string entityFileName, Action onClick)
    {
        entityNameText.text = entityName;
        entityFileNameText.text = entityFileName;
        rowButton.onClick.AddListener(() => onClick());
    }

    public void Select()
    {
        buttonImage.sprite = selectedSprite;
    }

    public void Deselect()
    {
        buttonImage.sprite = defaultSprite;
    }

    
}
