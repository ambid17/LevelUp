using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Linq;

public enum PropType
{
    AllDecorative,
    AllObstacles,
    Flowers,
    Plants,
    Grass,
    Rocks,
}

public class RoomPropGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform propParent;
    [SerializeField]
    private Transform obstacleParent;
    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private SpriteRenderer propPrefab;
    [SerializeField]
    private SpriteRenderer obstaclePrefab;

    [SerializeField]
    private PropType propType;
    [SerializeField]
    private bool isCluster;
    [SerializeField]
    private float clusterOffset;
    [SerializeField]
    private float clusterSizeOffset;
    [SerializeField]
    private bool isObstacle;
    [SerializeField]
    private float colliderCheckRadius;
    [SerializeField]
    private int numberToSpawn;
    [SerializeField]
    private int maxFailuresBeforeAbort;
    [SerializeField]
    private LayerMask layersToCauseFailure;
    [SerializeField]
    private RoomSpriteSettings roomSpriteSettings;

    private Vector2 _randomWithinTilemap => new Vector2(Random.Range(tileMap.cellBounds.min.x, tileMap.cellBounds.max.x), Random.Range(tileMap.cellBounds.min.y, tileMap.cellBounds.max.y));

    [InspectorButton("GenerateProps")]
    public bool generateProps;


    public void GenerateProps()
    {
        if (isCluster)
        {
            GenerateCluster();
        }
        else
        {
            GenerateIndividual();
        }
    }

    private void GenerateCluster()
    {
        //get all of the room sprites that have the correct prop type
        List<Sprite> spritePool = roomSpriteSettings.RoomSprites.Where(rs => rs.propType == propType).Select(rs => rs.sprite).ToList();

        Vector2 initSpawn = _randomWithinTilemap;
         int failures = 0;
         while (Physics2D.OverlapCircle(initSpawn, colliderCheckRadius, layersToCauseFailure))
         {
             initSpawn = _randomWithinTilemap;
             if (failures >= maxFailuresBeforeAbort)
             {
                 throw new Exception("Could not find unobstructed location in " + failures.ToString() + " tries. Try reducing radius or increasing max tries");
             }
             failures++;
             continue;
         }
         Transform parent = isObstacle ? obstacleParent : propParent;
         SpriteRenderer prefab = isObstacle ? obstaclePrefab : propPrefab;
        
         Vector2 newSpawn = initSpawn;
         for (int i = 0; i < numberToSpawn; i++)
         {
            SpriteRenderer renderer = Instantiate(prefab, newSpawn, Quaternion.identity, parent);
            renderer.sprite = spritePool[Random.Range(0, spritePool.Count)];
            float randomSizeOffset = Random.Range(-clusterSizeOffset, clusterSizeOffset);
            Vector3 sizeOffset = new(randomSizeOffset, randomSizeOffset);
            renderer.transform.localScale += sizeOffset;
            newSpawn = new Vector2(initSpawn.x + Random.Range(-clusterOffset, clusterOffset), initSpawn.y + Random.Range(-clusterOffset, clusterOffset));
        }
    }

    private void GenerateIndividual()
    {
        //get all of the room sprites that have the correct prop type
        List<Sprite> spritePool = roomSpriteSettings.RoomSprites.Where(rs => rs.propType == propType).Select(rs => rs.sprite).ToList();

        Transform parent = isObstacle ? obstacleParent : propParent;
        SpriteRenderer prefab = isObstacle ? obstaclePrefab : propPrefab;

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 newSpawn = _randomWithinTilemap;
            int failures = 0;
            while (Physics2D.OverlapCircle(newSpawn, colliderCheckRadius, layersToCauseFailure))
            {
                newSpawn = _randomWithinTilemap;
                if (failures >= maxFailuresBeforeAbort)
                {
                    throw new Exception("Could not find unobstructed location in " + failures.ToString() + " tries. Try reducing radius, increasing max tries, or decreasing number to spawn");
                }
                failures++;
                continue;
            }
            SpriteRenderer renderer = Instantiate(prefab, newSpawn, Quaternion.identity, parent);
            renderer.sprite = spritePool[Random.Range(0, spritePool.Count)];
        }
    }
}
