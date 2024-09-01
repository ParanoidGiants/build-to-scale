using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class WaveManager : MonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance => _instance;
    public Tilemap gridTilemap;
    public GameObject enemyPrefab;
    public int minimumEnemyPerWave = 1;
    public int maximumEnemyPerWave = 1;
    public float timeBetweenWaves = 1f;
    public float time = 0f;
    public List<Enemy> enemies;
    public bool isPreparingNewWave = true;
    private UIWave _uiWave;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        enemies = new List<Enemy>();
        _uiWave = FindObjectOfType<UIWave>();
    }
    void Start()
    {
        gridTilemap.CompressBounds();
    }

    void Update()
    {
        SpawnWave();
    }

    public void RemoveDeadEnemies()
    {
        for (int i = enemies.Count() - 1; i >= 0; --i)
        {
            var enemy = enemies[i];
            if (enemy == null)
            {
                enemies.Remove(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            isPreparingNewWave = true;
        }
    }

    private void SpawnWave()
    {
        RemoveDeadEnemies();
        if (!isPreparingNewWave)
        {
            return;
        }

        time += Time.deltaTime;
        _uiWave.SetTimeText(timeBetweenWaves - time);
        if (time < timeBetweenWaves)
        {
            return;
        }

        var numberOfEnemies = UnityEngine.Random.Range(minimumEnemyPerWave, maximumEnemyPerWave);
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
            var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform)
                .GetComponent<Enemy>();
            enemies.Add(enemy);

        }
        
        time = 0;
        minimumEnemyPerWave = (int) Mathf.Ceil(minimumEnemyPerWave * 1.1f);
        maximumEnemyPerWave = (int) Mathf.Ceil(maximumEnemyPerWave * 1.1f);
        isPreparingNewWave = false;
        _uiWave.SetEnemiesAttackingText();
    }

    public Enemy GetClosestEnemy(Vector3 hitWorldPosition, int radius)
    {
        var potentialEnemies = Physics2D.OverlapCircleAll(hitWorldPosition, radius);
        Enemy closestEnemy = null;
        var closestEnemyDistance = Mathf.Infinity;
        foreach (var potentialEnemy in potentialEnemies)
        {
            var enemy = potentialEnemy.GetComponent<Enemy>();
            if (enemy == null)
            {
                continue;
            }

            var currentEnemyDistance = Vector3.Distance(enemy.transform.position, hitWorldPosition);

            if (currentEnemyDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
                closestEnemyDistance = currentEnemyDistance;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy;
        }
        return null;
    }
}
