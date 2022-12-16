using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtils
{
    public static List<string> magnitudes = new List<string>()
    {
        "", "K", "M", "B", "T", "Q", "QQ", "S", "SS"
    };
    
    public static string ToCurrencyString(this float currency)
    {
        int index = 0;
        while (currency > 1000)
        {
            currency /= 1000;
            index++;
        }

        return $"{currency:0.###}{magnitudes[index]}";
    }    
}
