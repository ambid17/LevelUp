using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileSelector : MonoBehaviour
{
    [SerializeField] private Transform fileContainer;
    [SerializeField] private Transform fileButtonPrefab;
    [SerializeField] private GameObject visuals;

    [SerializeField] private Button cancelButton;
    [SerializeField] private Button selectButton;

    private void Start()
    {
        cancelButton.onClick.AddListener(Cancel);
        selectButton.onClick.AddListener(Select);
    }

    public void ChangeEntity()
    {
        visuals.SetActive(true);
    }

    private void Cancel()
    {

    }

    private void Select()
    {
        visuals.SetActive(false);
    }
}
