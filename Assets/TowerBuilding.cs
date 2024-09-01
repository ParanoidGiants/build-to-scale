using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerBuilding : MonoBehaviour
{
    private static uint TOWER_ID_SOURCE = 2;
    private uint _id; 
    public uint Id => _id; 
    private Tilemap _tilemap;
    public Tilemap BuildingTilemap => _tilemap;
    public GameObject rangeTilemap;
    public Transform cannon;
    public GameObject projectilePrefab;
    public bool isRangeVisible = false;
    public float attackRange = 0f;
    public List<Enemy> enemiesInRange;
    public int maxHitPoints = 1;
    private int _hitPoints;
    public Vector3Int cellIndex; 
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        enemiesInRange = new List<Enemy>();
        _hitPoints = maxHitPoints;
        _id = TOWER_ID_SOURCE;
        ++TOWER_ID_SOURCE;
        Debug.Log($"Tower {_id} spawned");
    }
    void Start()
    {
        _tilemap.CompressBounds();
    }
    private void OnDestroy()
    {
        Debug.Log($"Tower {_id} destroyed");
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
        for (int i = enemiesInRange.Count - 1; i >= 0; --i)
        {
            var enemy = enemiesInRange[i];
            if (enemy == null)
            {
                enemiesInRange.Remove(enemy);
                continue;
            }
            float enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
            if (enemyDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
                closestEnemyDistance = enemyDistance;
            }
        }
        if (enemiesInRange.Count == 0)
        {
            return;
        }
        var lookDirection = closestEnemy.transform.position - transform.position;
        cannon.transform.up = lookDirection;
    }
    
    public bool IsBuildingOnWorldPosition(Vector3 worldPosition)
    {
        Debug.Log($"TOWER: {_id} queried");
        var cellIndex = _tilemap.WorldToCell(worldPosition);
        return _tilemap.GetTile(cellIndex) != null;
    }

    public void AddEnemy(Enemy enemy)
    {
        enemiesInRange.Add(enemy);
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        Hit(enemy.hitPoints);
        enemy.Hit(enemy.hitPoints);
    }

    public int Hit(int damage)
    {
        var dealtDamage = Mathf.Min(damage, _hitPoints);
        _hitPoints -= damage;
        if (_hitPoints <= 0)
        {
            BuildingManager.Instance.RemoveTower(cellIndex);
        }
        return dealtDamage;
    }
}
