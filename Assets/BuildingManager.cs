using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager _instance;
    public static BuildingManager Instance => _instance;
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap placementTilemap;
    public TileBase placementTile;
    public BaseBuilding baseBuilding;
    public List<TowerBuilding> towers;
    public GameObject towerPrefab;
    public int maxPlacementCount = 0;
    public int currentPlacementCount = 0;
    public uint[,] buildingFlags = new uint[0,0];
    private BoundsInt _groundBounds;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        placementTilemap.CompressBounds();
        groundTilemap.CompressBounds();
    }

    private void Start()
    {
        // UpdateBuildingFlags
        RefreshBuildingFlags();
    }

    private void RefreshBuildingFlags()
    {
        _groundBounds = groundTilemap.cellBounds;
        var width = _groundBounds.size.x;
        var height = _groundBounds.size.y;
        buildingFlags = new uint[width, height];
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                buildingFlags[i,j] = 0;
            }
        }

        var baseIndexX = _groundBounds.size.x / 2;
        var baseIndexY = _groundBounds.size.y / 2;
        buildingFlags[baseIndexX,baseIndexY]
            = buildingFlags[baseIndexX,baseIndexY-1]
            = buildingFlags[baseIndexX-1,baseIndexY]
            = buildingFlags[baseIndexX-1,baseIndexY-1]
            // ID of the Base Building
            = 1;

        foreach (var tower in towers)
        {
            SetTowerBuildingFlag(tower);
        }
    }

    private void SetTowerBuildingFlag(TowerBuilding tower)
    {
        var cellIndex = tower.cellIndex;
        var i = cellIndex.x - _groundBounds.xMin;
        var j = cellIndex.y - _groundBounds.yMin;
        buildingFlags[i, j] = tower.Id;
    }

    private void RemoveTowerBuildingFlag(TowerBuilding tower)
    {
        var cellIndex = tower.cellIndex;
        var i = cellIndex.x - _groundBounds.xMin;
        var j = cellIndex.y - _groundBounds.yMin;
        buildingFlags[i, j] = 0;
    }

    public void ShowPlacementGrid()
    {
        placementTilemap.ClearAllTiles();
        placementTilemap.gameObject.SetActive(true);
        for (int x = _groundBounds.xMin; x < _groundBounds.xMax; ++x) 
        { 
            for (int y = _groundBounds.yMin; y < _groundBounds.yMax - 1; ++y) 
            {
                var cellIndex = new Vector3Int(x,y);
                var colorTile = CanBuildOnCell(cellIndex);
                if (!colorTile)
                {
                    continue;
                }
                placementTilemap.SetTile(cellIndex, placementTile);
            } 
        }
    }
    
    public void HidePlacementGrid()
    {
        placementTilemap.gameObject.SetActive(false);
    }

    public bool IsBuildingOnGridCell(Vector3Int cellIndex)
    {
        var i = cellIndex.x - _groundBounds.xMin;
        var j = cellIndex.y - _groundBounds.yMin;
        return buildingFlags[i,j] != 0;
    }
    

    public bool IsTowerOnGridCell(Vector3Int cellIndex)
    {
        var i = cellIndex.x - _groundBounds.xMin;
        var j = cellIndex.y - _groundBounds.yMin;
        if (buildingFlags[i,j] == 0)
        {
            return false;
        }

        var buildingId = buildingFlags[i,j];
        return towers.Any(t => t.Id == buildingId);
    }

    public void RemoveTower(Vector3Int cellIndex)
    {
        var tower = towers.FirstOrDefault(t => t.IsBuildingOnWorldPosition(cellIndex));
        if (tower == null)
        {
            Debug.LogWarning($"Tried to remove a not existing tower from {cellIndex}");
        }
        currentPlacementCount--;
        towers.Remove(tower);
        RemoveTowerBuildingFlag(tower);
        RefreshBuildingFlags();
        ShowPlacementGrid();
        Destroy(tower.gameObject);
    }

    public void AddTower(Vector3Int cellIndex)
    {
        currentPlacementCount++;
        var newTower = Instantiate(
            towerPrefab,
            grid.CellToWorld(cellIndex),
            Quaternion.identity,
            transform
        ).GetComponent<TowerBuilding>();
        newTower.cellIndex = cellIndex;
        SetTowerBuildingFlag(newTower);
        towers.Add(newTower);
        RefreshBuildingFlags();
        ShowPlacementGrid();
    }

    internal bool CanBuildOnCell(Vector3Int cellIndex)
    {
        var i = cellIndex.x - _groundBounds.xMin;
        var j = cellIndex.y - _groundBounds.yMin;
        Debug.Log($"Bounds {_groundBounds}");
        Debug.Log($"Check cell {cellIndex}");
        Debug.Log($"For Index ({i}, {j})");
        var isBuildingOnTile = buildingFlags[i,j] != 0;
        if (isBuildingOnTile)
        {
            return false;
        }
        
        int iMin = Mathf.Max(i-1, 0);
        int jMin = Mathf.Max(j-1, 0);
        int iMax = Mathf.Min(i+1, buildingFlags.GetLength(0)-1);
        int jMax = Mathf.Min(j+1, buildingFlags.GetLength(1)-1);
        
        var isBuildingOnNeighbouringTile = false;
        for (int m = iMin; m <= iMax; ++m)
        {
            for (int n = jMin; n <= jMax; ++n)
            {
                if (buildingFlags[m,n] != 0)
                {
                    isBuildingOnNeighbouringTile = true;
                    break;
                }
            }
            if (isBuildingOnNeighbouringTile)
            {
                break;
            }
        }
        return isBuildingOnNeighbouringTile;
    }

    public void HandleClickOnCell(Vector3Int cellIndex)
    {
        if (IsTowerOnGridCell(cellIndex))
        {
            RemoveTower(cellIndex);
        }
        else if (CanBuildOnCell(cellIndex))
        {
            AddTower(cellIndex);
        }
    }
}
