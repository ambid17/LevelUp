using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressModel
{
    public float Currency;
    public WorldData WorldData;
}

public class WorldData
{
    public List<CountryData> CountryDatas;
}

public class CountryData
{
    public int kills;
}
