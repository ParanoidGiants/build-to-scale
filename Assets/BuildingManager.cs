using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager _instance;
    public static BuildingManager Instance => _instance;
    public Grid grid;
    public BaseBuilding baseBuilding;
    public List<TowerBuilding> towers;
    public GameObject towerPrefab;
    public int maxPlacementCount = 0;
    public int currentPlacementCount = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        _instance = this;
    }

    public bool IsBuildingOnGridCell(Vector3Int cellIndex)
    {
        return IsTowerOnGridCell(cellIndex) || IsBaseOnGridCell(cellIndex);
    }

    public bool IsTowerOnGridCell(Vector3Int cellIndex)
    {
        var worldPosition = grid.CellToWorld(cellIndex);
        foreach (var tower in towers)
        {
            if (tower.IsBuildingOnWorldPosition(worldPosition))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsBaseOnGridCell(Vector3Int cellIndex)
    {
        var worldPosition = grid.CellToWorld(cellIndex);
        return baseBuilding.IsBuildingOnWorldPosition(worldPosition);
    }

    public bool IsBuildingOnWorldPosition(Vector3 worldPosition)
    {
        var cellIndex = grid.WorldToCell(worldPosition);
        return IsBuildingOnGridCell(cellIndex);
    }

    public void RemoveTower(Vector3Int cellIndex)
    {
        foreach (var tower in towers)
        {
            if (tower.IsBuildingOnWorldPosition(cellIndex))
            {
                towers.Remove(tower);
                Destroy(tower.gameObject);
                currentPlacementCount--;
                return;
            }
        }
    }

    public bool CanBuild()
    {
        return currentPlacementCount < maxPlacementCount;
    }

    public void AddTower(Vector3Int cellIndex)
    {
        var worldPosition = grid.CellToWorld(cellIndex);
        var newTower = Instantiate(towerPrefab, worldPosition, Quaternion.identity, transform);
        towers.Add(newTower.GetComponent<TowerBuilding>());
        currentPlacementCount++;
    }

    internal bool CanBuildOnCell(Vector3Int cellIndex)
    {
        return currentPlacementCount < maxPlacementCount && !IsBuildingOnGridCell(cellIndex);
    }

    public void HandleClickOnCell(Vector3Int currentCell)
    {
        if (CanBuildOnCell(currentCell))
        {
            AddTower(currentCell);
        }
        else if (IsTowerOnGridCell(currentCell))
        {
            RemoveTower(currentCell);
        }
    }
}
