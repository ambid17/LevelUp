using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class LoadingUIController : MonoBehaviour
{
    public TMP_Text loadingText;

    private int dotCount;

    private float dotInterval = 0.4f;
    private float dotTimer;
    void Start()
    {
        
    }

    void Update()
    {
        dotTimer += Time.deltaTime;

        if (dotTimer > dotInterval)
        {
            dotTimer = 0;
            if (dotCount == 3)
            {
                dotCount = 1;
            }
            else
            {
                dotCount++;
            }
        }
        
        string dots = string.Empty;
        for (int i = 0; i < dotCount; i++)
        {
            dots += ".";
        }

        loadingText.text = $"Loading{dots}";
    }
}
