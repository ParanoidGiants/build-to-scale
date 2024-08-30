using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public Tilemap gridTilemap;
    public GameObject enemyPrefab;
    public int minimumEnemyPerWave = 1;
    public int maximumEnemyPerWave = 1;
    public float timeBetweenWaves = 1f;
    public float time = 0f;

    void Start()
    {
        gridTilemap.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!AreEnemiesDead())
        // {
        //     return;
        // }
        time += Time.deltaTime;
        
        if (time > timeBetweenWaves)
        {
            var numberOfEnemies = UnityEngine.Random.Range(1, maximumEnemyPerWave);
            var xMinPosition = gridTilemap.CellToWorld(gridTilemap.cellBounds.min).x;
            var xMaxPosition = gridTilemap.CellToWorld(gridTilemap.cellBounds.max).x;
            var yMinPosition = gridTilemap.CellToWorld(gridTilemap.cellBounds.min).y;
            var yMaxPosition = gridTilemap.CellToWorld(gridTilemap.cellBounds.max).y;
            for (int i = 0; i < numberOfEnemies; ++i)
            {
                var random = UnityEngine.Random.Range(0f, 1f);
                Vector3 spawnPosition;
                if (random < 0.25f)
                {
                    var xPosition = xMinPosition;
                    var yPosition = UnityEngine.Random.Range(
                        yMinPosition,
                        yMaxPosition
                    );
                    spawnPosition = new Vector3(xPosition, yPosition, 0f);
                }
                else if (random < 0.5f)
                {
                    var xPosition = xMaxPosition;
                    var yPosition = UnityEngine.Random.Range(
                        yMinPosition,
                        yMaxPosition
                    );
                    spawnPosition = new Vector3(xPosition, yPosition, 0f);
                }
                else if (random < 0.75f)
                {
                    var xPosition = UnityEngine.Random.Range(
                        xMinPosition,
                        xMaxPosition
                    );
                    var yPosition = yMinPosition;
                    spawnPosition = new Vector3(xPosition, yPosition, 0f);
                }
                else
                {
                    var xPosition = UnityEngine.Random.Range(
                        xMinPosition,
                        xMaxPosition
                    );
                    var yPosition = yMaxPosition;
                    spawnPosition = new Vector3(xPosition, yPosition, 0f);
                }
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            }
            
            time = 0;
            minimumEnemyPerWave = (int) Mathf.Ceil(minimumEnemyPerWave * 1.1f);
            maximumEnemyPerWave = (int) Mathf.Ceil(maximumEnemyPerWave * 1.1f);
        }
    }
}
