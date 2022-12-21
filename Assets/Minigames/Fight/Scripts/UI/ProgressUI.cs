using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private TMP_Text worldNameText;
    [SerializeField] private Image worldImage;
    [SerializeField] private Slider areaProgressSlider;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetWorld(World world)
    {
        worldNameText.text = world.Name;
        worldImage.sprite = world.WorldSprite;
    }

    public void UpdateAreaProgress(float percentComplete)
    {
        areaProgressSlider.value = percentComplete;
    }
}
