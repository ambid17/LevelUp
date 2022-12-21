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
        //Randomizes the first terrain chunk from the blank terrain list so you don't get spawn camped
        int firstSpawn = Random.Range(0, TerrainManager.BlankTerrain.Count);
        //sets the first chunk of terrain to spawn directly beneath the player
        Vector2 initSpawnPos = new Vector2(PlayerPos().x, PlayerPos().y - initSpawnHeight);
        Tilemap initTerrain = TerrainManager.BlankTerrain[firstSpawn];
        //instantiates initial terrain chunk and assigns it to the singleton
        TerrainManager.CurrentTerrain = Instantiate(initTerrain, initSpawnPos, initTerrain.transform.rotation, TerrainManager.Grid);
        //generates the rest of the terrain up to the max distance before first frame
        while (Vector2.Distance(PlayerPos(), TerrainManager.CurrentTerrain.transform.position) < spawnDist)
        {
            RandomizeTerrain();
        }
    }
    private void Update()
    {
        //spawns new terrain as player vehicle approaches
        if (Vector2.Distance(PlayerPos(), TerrainManager.CurrentTerrain.transform.position) < spawnDist)
        {
            RandomizeTerrain();
        }
    }
    private void RandomizeTerrain()
    {
        //DnD style encounter check, roll a D20 to determine if you get a hazard, enemy, both, or nothing
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
        //passes in the correct list to spawn from
        SpawnTerrain(tempList);
    }
    private void SpawnTerrain(List<Tilemap> list)
    {
        //randomizes an entry from the provided list
        int RNG = Random.Range(0, list.Count);
        //calculates the current rightmost edge of the map in world space
        float currentEdge = TerrainManager.CurrentTerrain.transform.TransformPoint(TerrainManager.CurrentTerrain.cellBounds.max).x;
        //calculates the distance between the leftmost edge of the next Tilemap to be instantiated, and that Tilemap's transform
        float newDiff = Mathf.Abs(list[RNG].cellBounds.xMin - list[RNG].transform.position.x);
        //determines the correct position on the X axis for the next Tilemap based on the calculations
        float newX = currentEdge + newDiff;
        Vector2 newPos = new Vector2(newX, TerrainManager.CurrentTerrain.transform.position.y);
        //instantiates the new map in the correct position and sets it as the new current terrain in the singleton
        TerrainManager.CurrentTerrain = Instantiate(list[RNG], newPos, list[RNG].transform.rotation, TerrainManager.Grid);
    }
    private Vector2 PlayerPos()
    {
        return TerrainManager.VehicleBody.transform.position;
    }
}
