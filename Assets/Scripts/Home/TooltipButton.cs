using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    [SerializeField] private string _header;
    [SerializeField] private string _enabledContent;
    [SerializeField] private string _disabledContent;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string content = string.Empty;
        
        if (_button.interactable)
        {
            content = _enabledContent;
        }
        else
        {
            content = _disabledContent;
        }
        
        Platform.EventService.Dispatch(new TooltipShowEvent(_header, content));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // The OnPointerExit triggers when leaving the scene
        // This asks for the EventService which can be destroyed, so if we are loading a scene don't do anything
        if (HomeManager.IsLoadingScene)
        {
            return;
        }
        Platform.EventService.Dispatch<TooltipHideEvent>();
    }
}
