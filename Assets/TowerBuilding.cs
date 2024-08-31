using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerBuilding : MonoBehaviour
{
    private Tilemap _tilemap;
    public Tilemap BuildingTilemap => _tilemap;
    public GameObject rangeTilemap;
    public Transform cannon;
    public GameObject projectilePrefab;
    public bool isRangeVisible = false;
    public float attackRange = 0f;
    public List<Enemy> enemiesInRange;
    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemap.CompressBounds();
        enemiesInRange = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        rangeTilemap.SetActive(isRangeVisible);
        if (enemiesInRange.Count == 0)
        {
            return;
        }
        Enemy closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;
        foreach (var enemy in enemiesInRange)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
            if (enemyDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
            }
        }
        var lookDirection = closestEnemy.transform.position - transform.position;
        cannon.transform.up = lookDirection;
    }
    
    public bool IsBuildingOnWorldPosition(Vector3 worldPosition)
    {
        var cellIndex = _tilemap.WorldToCell(worldPosition);
        Debug.Log($"TOWER: {cellIndex}");
        return _tilemap.GetTile(cellIndex) != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }
        enemiesInRange.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }
        enemiesInRange.Remove(enemy);
    }
}
