using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DuplicateStatsPopup : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject visuals;
    [SerializeField] private DevCheatsUI devCheatsUI;

    private Entity toDuplicate;

    private void Start()
    {
        okButton.onClick.AddListener(Ok);
        cancelButton.onClick.AddListener(Cancel);
        inputField.onValueChanged.AddListener(delegate { ValidateInput(); });
    }

    public void Show(Entity toDuplicate)
    {
        visuals.SetActive(true);
        this.toDuplicate = toDuplicate;
    }

    private void Ok() 
    {
        var filePath = Path.Combine(FightDataLoader.GetEntityFolderPath(), $"{inputField.text}.json");
        FileUtils.SaveFile(filePath, toDuplicate.Stats);
        Debug.Log($"Saved entityStats duplicate of ");

        inputField.text = string.Empty;
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }

    private void Cancel()
    {
        inputField.text = string.Empty;
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }

    private void ValidateInput()
    {
        var textToValidate = inputField.text;
        var filePath = Path.Combine(FightDataLoader.GetEntityFolderPath(), $"{textToValidate}.json");

        if (string.IsNullOrEmpty(textToValidate))
        {
            okButton.interactable = false;
        }
        else if (File.Exists(filePath))
        {
            okButton.interactable = false;
        }
        else
        {
            okButton.interactable = true;
        }
    }
}
