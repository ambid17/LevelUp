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
            SpawnTerrain();
        }
    }
    private void SpawnTerrain()
    {
        int RNG = Random.Range(0, TerrainManager.BlankTerrain.Count);
        float currentEdge = TerrainManager.CurrentTerrain.transform.TransformPoint(TerrainManager.CurrentTerrain.cellBounds.max).x;
        float newDiff = Mathf.Abs(TerrainManager.BlankTerrain[RNG].GetComponent<Tilemap>().cellBounds.xMin - TerrainManager.BlankTerrain[RNG].transform.position.x);
        float newX = currentEdge + newDiff;
        Vector2 newPos = new Vector2(newX, TerrainManager.CurrentTerrain.transform.position.y);
        TerrainManager.CurrentTerrain = Instantiate(TerrainManager.BlankTerrain[RNG], newPos, TerrainManager.BlankTerrain[RNG].transform.rotation, TerrainManager.Grid);
    }
    private Vector2 PlayerPos()
    {
        return TerrainManager.VehicleBody.transform.position;
    }
}
