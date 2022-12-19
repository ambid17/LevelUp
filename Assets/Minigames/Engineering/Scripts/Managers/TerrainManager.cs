using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    public VehicleBody vehicle;

    public GameObject currentTerrain;

    public List<GameObject> blankTerrain;
    public List<GameObject> hazards;
    public List<GameObject> enemies;
}
