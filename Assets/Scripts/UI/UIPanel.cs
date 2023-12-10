using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private GameObject container;

    public virtual void Toggle(bool shouldBeActive)
    {
        container.SetActive(shouldBeActive);
    }
}
