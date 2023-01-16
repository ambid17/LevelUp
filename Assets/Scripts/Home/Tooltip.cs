using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private GameObject _container;
    [SerializeField] private int _characterWrapLimit;
    
    private LayoutElement _layoutElement;
    private RectTransform _containerRect;
    private Camera _camera;
    
    private EventService _eventService;
    
    void Start()
    {
        _layoutElement = GetComponentInChildren<LayoutElement>();
        _containerRect = _container.GetComponent<RectTransform>();
        _camera = Camera.main;
        
        _eventService = GameManager.EventService;
        _eventService.Add<TooltipShowEvent>(ShowTooltip);
        _eventService.Add<TooltipHideEvent>(HideTooltip);
        
        HideTooltip();
    }

    public void ShowTooltip(TooltipShowEvent eventType)
    {
        _container.SetActive(true);

        if (string.IsNullOrEmpty(eventType.Header))
        {
            _headerText.gameObject.SetActive(false);
        }
        else
        {
            _headerText.gameObject.SetActive(transform);
            _headerText.text = eventType.Header;
        }

        _contentText.text = eventType.Content;
        ToggleLayoutElement();
    }

    /// <summary>
    /// This allows the layout element to be toggled off so that the tooltip can shrink beyond the preferred width
    /// </summary>
    private void ToggleLayoutElement()
    {
        int headerLength = _headerText.text.Length;
        int contentLength = _contentText.text.Length;
        _layoutElement.enabled =
            (headerLength > _characterWrapLimit || contentLength > _characterWrapLimit) ? true : false;
    }

    private void HideTooltip()
    {
        _container.SetActive(false);
    }

    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        _containerRect.pivot = new Vector2(pivotX, pivotY);
        transform.position = Input.mousePosition;
    }
}
