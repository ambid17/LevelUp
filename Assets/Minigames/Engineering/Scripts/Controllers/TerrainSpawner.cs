using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnDist;
    [SerializeField]
    private float initSpawnHeight;

    private void Awake()
    {
        int firstSpawn = Random.Range(0, TerrainManager.BlankTerrain.Count);
        Vector2 initSpawnPos = new Vector2(PlayerPos().x, PlayerPos().y - initSpawnHeight);
        Tilemap initTerrain = TerrainManager.BlankTerrain[firstSpawn];
        TerrainManager.CurrentTerrain = Instantiate(initTerrain, initSpawnPos, initTerrain.transform.rotation, TerrainManager.Grid);
        while (Vector2.Distance(PlayerPos(), TerrainManager.CurrentTerrain.transform.position) < spawnDist)
        {
            RandomizeTerrain();
        }
    }
    private void Update()
    {
        if (Vector2.Distance(PlayerPos(), TerrainManager.CurrentTerrain.transform.position) < spawnDist)
        {
            RandomizeTerrain();
        }
    }
    private void RandomizeTerrain()
    {
        int RNG = Random.Range(0, 21);
        List<Tilemap> tempList = new List<Tilemap>();
        switch (RNG)
        {
            case 1:
                tempList = TerrainManager.BlankTerrain;
                break;
            case <= 5:
                tempList = TerrainManager.BlankTerrain;
                break;
            case <= 19:
                tempList = TerrainManager.BlankTerrain;
                break;
            case 20:
                tempList = TerrainManager.BlankTerrain;
                break;
        }
        SpawnTerrain(tempList);
    }
    private void SpawnTerrain(List<Tilemap> list)
    {
        int RNG = Random.Range(0, list.Count);
        float currentEdge = TerrainManager.CurrentTerrain.transform.TransformPoint(TerrainManager.CurrentTerrain.cellBounds.max).x;
        float newDiff = Mathf.Abs(list[RNG].cellBounds.xMin - list[RNG].transform.position.x);
        float newX = currentEdge + newDiff;
        Vector2 newPos = new Vector2(newX, TerrainManager.CurrentTerrain.transform.position.y);
        TerrainManager.CurrentTerrain = Instantiate(list[RNG], newPos, list[RNG].transform.rotation, TerrainManager.Grid);
    }
    private Vector2 PlayerPos()
    {
        return TerrainManager.VehicleBody.transform.position;
    }
}
