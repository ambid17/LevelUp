using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainManager : Singleton<TerrainManager>
{
    [SerializeField]
    private VehicleBody vehicle;

    [SerializeField]
    private Tilemap currentTerrain;

    [SerializeField]
    private Transform grid;

    [SerializeField]
    private List<Tilemap> blankTerrain;
    [SerializeField]
    private List<Tilemap> hazards;
    [SerializeField]
    private List<GameObject> enemies;

    public static VehicleBody VehicleBody => Instance.vehicle;
    public static Transform Grid => Instance.grid;
    public static List<Tilemap> BlankTerrain => Instance.blankTerrain;
    public static List<Tilemap> Hazards => Instance.hazards;
    public static List<GameObject> Enemies => Instance.enemies;
    public static Tilemap CurrentTerrain 
    {
        get 
        { 
            return Instance.currentTerrain; 
        }
        set
        {
            Instance.currentTerrain = value;
        }
    }
}
