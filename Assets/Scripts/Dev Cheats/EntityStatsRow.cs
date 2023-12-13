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

    public void Setup(string entityName, string entityFileName, Action onClick)
    {
        entityNameText.text = entityName;
        entityFileNameText.text = entityFileName;
        rowButton.onClick.AddListener(()  => onClick());
    }
}
