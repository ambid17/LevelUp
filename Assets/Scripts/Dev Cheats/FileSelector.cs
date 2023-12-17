using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileSelector : MonoBehaviour
{
    [SerializeField] private Transform fileContainer;
    [SerializeField] private FileSelectorButton fileButtonPrefab;
    [SerializeField] private GameObject visuals;

    [SerializeField] private Button cancelButton;
    [SerializeField] private Button selectButton;

    [SerializeField] private DevCheatsUI devCheatsUI;

    private string entityDirectory => Path.Combine(Application.persistentDataPath, FightDataLoader.ENTITY_FOLDER);

    private Entity selectedEntity;
    private string selectedFile;
    private List<FileSelectorButton> selectorButtons;

    private void Start()
    {
        cancelButton.onClick.AddListener(Cancel);
        selectButton.onClick.AddListener(Select);
    }

    public void SelectStatsForEntity(Entity entity)
    {
        visuals.SetActive(true);

        selectorButtons = new();

        foreach (var filePath in Directory.EnumerateFiles(entityDirectory))
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileButton = Instantiate(fileButtonPrefab, fileContainer);
            fileButton.buttonText.text = fileName;
            fileButton.button.onClick.AddListener(() => OnFileSelected(fileName, fileButton));
            selectorButtons.Add(fileButton);
        }
    }

    private void OnFileSelected(string file, FileSelectorButton button)
    {
        selectedFile = file;

        // deselect all other buttons
        foreach(var selectorButton in selectorButtons)
        {
            selectorButton.Deselect();
        }

        button.Select();
    }

    private void Cancel()
    {
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }

    private void Select()
    {
        FightDataLoader.Instance.UpdateRemapping(selectedEntity.statsFileName, selectedFile);
        visuals.SetActive(false);
        devCheatsUI.OnFinishOperation();
    }
}
