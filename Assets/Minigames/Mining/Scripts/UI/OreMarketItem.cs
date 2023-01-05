using Minigames.Mining;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class OreMarketItem : MonoBehaviour
{
    private TileDescriptor descriptor;
    [SerializeField] Image _image;
    [SerializeField] TMP_Text _oreName;
    [SerializeField] TMP_Text _oreCount;
    [SerializeField] TMP_Text _sellText;
    [SerializeField] Button _sellButton;
    [SerializeField] Slider _sellSlider;
    
    void OnEnable()
    {
        UpdateItem();
    }
    
    private void Start()
    {
        _sellSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _sellButton.onClick.AddListener(OnSellButtonPress);
    }

    public void Setup(TileDescriptor descriptor)
    {
        this.descriptor = descriptor;
        _image.sprite = descriptor.Tile.sprite;
        UpdateItem();
    }
    void SetSaleText()
    {
        float saleValue = _sellSlider.value * descriptor.Value;
        _sellText.text = $"Sell for " + saleValue.ToCurrencyString();
    }

    private void OnSellButtonPress()
    {
        descriptor.Count -= (int)_sellSlider.value;
        GameManager.Currency += _sellSlider.value * descriptor.Value;
        UpdateItem();
    }


    void OnSliderValueChanged(float newValue)
    {
        if (newValue == 0) _sellButton.interactable = false;
        else _sellButton.interactable = true;
        SetSaleText();
    }

    void UpdateItem()
    {
        if (GameManager.Instance == null || descriptor == null) return;
        SetSaleText();
        _sellSlider.maxValue = descriptor.Count;
        _sellSlider.value = 0;
        _oreName.text = descriptor.TileType.ToString();
        _oreCount.text = descriptor.Count.ToCurrencyString();
    }
}
