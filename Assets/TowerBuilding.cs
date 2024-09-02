using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerBuilding : MonoBehaviour
{
    private static uint TOWER_ID_SOURCE = 2;
    private uint _id; 
    public uint Id => _id; 
    private Tilemap _tilemap;
    public List<Enemy> enemiesInRange;
    public int maxHitPoints = 1;
    public int hitPoints;
    public Vector3Int cellIndex;
    public Transform cannonTransform;
    public TowerCannon cannon;
    public float cannonShootIntervalTime = 1f;
    public float cannonShootTime = 0f;
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        enemiesInRange = new List<Enemy>();
        hitPoints = maxHitPoints;
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
    
    private int SortEnemiesByDistance(Enemy a, Enemy b)
    {
        float enemyDistanceA = Vector3.Distance(a.transform.position, transform.position);
        float enemyDistanceB = Vector3.Distance(b.transform.position, transform.position);
        return enemyDistanceA > enemyDistanceB ? 1 : -1;
    }

    void Update()
    {
        enemiesInRange.RemoveAll(e => e == null);
        cannonShootTime += Time.deltaTime;
        if (enemiesInRange.Count == 0)
        {
            return;
        }
        enemiesInRange.Sort(SortEnemiesByDistance);
        Enemy closestEnemy = enemiesInRange[0];
        // for (int i = enemiesInRange.Count - 1; i >= 0; --i)
        // {
        //     var enemy = enemiesInRange[i];
        //     if (enemy == null)
        //     {
        //         enemiesInRange.Remove(enemy);
        //         continue;
        //     }
        //     float enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
        //     if (enemyDistance < closestEnemyDistance)
        //     {
        //         closestEnemy = enemy;
        //         closestEnemyDistance = enemyDistance;
        //     }
        // }
        cannon.UpdateRotation(closestEnemy);
        if (cannonShootTime < cannonShootIntervalTime)
        {
            return;
        }

        cannonShootTime = 0f;
        cannon.Shoot(closestEnemy.transform);
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
        var dealtDamage = Mathf.Min(damage, hitPoints);
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            BuildingManager.Instance.RemoveTower(cellIndex);
        }
        return dealtDamage;
    }
}
