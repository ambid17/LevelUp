using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressModel
{
    public List<WorldData> WorldData;
}

public class WorldData
{
    public string WorldName;
    public List<CountryData> CountryData;
    public float Currency;
    public float CurrencyPerMinute;
    public DateTime LastTimeVisited;

}

public class CountryData
{
    public int CountryIndex;
    public int Kills;
}
