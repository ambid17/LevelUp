using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Linq;
using System.Runtime.CompilerServices;

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

        Vector2 initSpawn = GetRandomInTilemap();
        int failures = 0;

        Transform parent = isObstacle ? obstacleParent : propParent;
        SpriteRenderer prefab = isObstacle ? obstaclePrefab : propPrefab;

        SpriteRenderer renderer = Instantiate(prefab, initSpawn, Quaternion.identity, parent);

        var collider = renderer.GetComponent<Collider2D>();
        var colList = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.SetLayerMask(layersToCauseFailure);
        filter.useTriggers = true;

        collider.OverlapCollider(filter, colList);

        while (colList.Count > 0)
        {
            renderer = Instantiate(prefab, initSpawn, Quaternion.identity, parent);
            DestroyImmediate(renderer.gameObject);
            initSpawn = GetRandomInTilemap();
            
            // If there are no acceptable locations while loop will go on forever, this check prevents freezes.
            if (failures >= maxFailuresBeforeAbort)
            {
                throw new Exception("Could not find unobstructed location in " + failures.ToString() + " tries. Try reducing radius or increasing max tries");
            }
            failures++;
            continue;
        }

        Vector2 newSpawn = initSpawn;
        for (int i = 0; i < numberToSpawn; i++)
        {
            // Make the entire cluster children of the first renderer to make it more readable in the editor.
            SpriteRenderer rendererCluster = Instantiate(prefab, newSpawn, Quaternion.identity, renderer.transform);
            rendererCluster.sprite = spritePool[Random.Range(0, spritePool.Count)];
            float randomSizeOffset = Random.Range(-clusterSizeOffset, clusterSizeOffset);
            Vector3 sizeOffset = new(randomSizeOffset, randomSizeOffset);
            rendererCluster.transform.localScale += sizeOffset;
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
            Vector2 newSpawn = GetRandomInTilemap();
            var col = Physics2D.OverlapBox(newSpawn, new Vector2(colliderCheckRadius, colliderCheckRadius), layersToCauseFailure);

            SpriteRenderer renderer = Instantiate(prefab, newSpawn, Quaternion.identity, parent);

            var collider = renderer.GetComponent<Collider2D>();
            var colList = new List<Collider2D>();
            var filter = new ContactFilter2D();
            filter.SetLayerMask(layersToCauseFailure);
            filter.useTriggers = true;

            collider.OverlapCollider(filter, colList);

            if(colList.Count > 0)
            {
                DestroyImmediate(renderer.gameObject);
                continue;
            }

            collider.transform.position += new Vector3(collider.offset.x, collider.offset.y, 0);
            collider.offset = Vector2.zero;
            renderer.gameObject.layer = 10; // TODO: pick correct layer
            renderer.sprite = spritePool[Random.Range(0, spritePool.Count)];
        }
    }

    private Vector3 GetRandomInTilemap()
    {
        // Cast to float so it uses Random.Range(float, float). Otherwise they are snapped to the tilemap
        float x = Random.Range((float)tileMap.cellBounds.min.x, tileMap.cellBounds.max.x);
        float y = Random.Range((float)tileMap.cellBounds.min.y, tileMap.cellBounds.max.y);

        return new Vector3(x, y, 0);
    }
}
