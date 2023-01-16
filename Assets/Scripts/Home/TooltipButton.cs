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
        
        GameManager.EventService.Dispatch(new TooltipShowEvent(_header, content));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.EventService.Dispatch<TooltipHideEvent>();
    }
}
