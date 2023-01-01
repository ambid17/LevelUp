using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressModel
{
    public float Currency;
    public List<WorldData> WorldData;
}

public class WorldData
{
    public string worldName;
    public List<CountryData> CountryData;
}

public class CountryData
{
    public int countryIndex;
    public int kills;
}
