using Minigames.Mining;
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

    public void Setup(TileDescriptor descriptor)
    {
        this.descriptor = descriptor;
        _image.sprite = descriptor.Tile.sprite;
        _oreName.text = descriptor.TileType.ToString();
        _oreCount.text = descriptor.Count.ToCurrencyString();
        SetSaleText();
    }
    void SetSaleText()
    {
        float saleValue = _sellSlider.value * descriptor.Count * descriptor.Value;
        _sellText.text = $"Sell for " + saleValue.ToCurrencyString();
    }
    private void Start()
    {
        _sellSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    void OnSliderValueChanged(float newValue)
    {
        if (newValue == 0) _sellButton.interactable = false;
        else _sellButton.interactable = true;
        SetSaleText();
    }
}
