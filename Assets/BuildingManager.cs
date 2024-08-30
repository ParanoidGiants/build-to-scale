using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Grid grid;
    public BaseBuilding baseBuilding;
    public List<TowerBuilding> towers;
    public GameObject towerPrefab;
    public int maxPlacementCount = 2;
    public int currentPlacementCount = 0;

    public bool IsBuildingOnGridCell(Vector3Int cellIndex)
    {
        return IsTowerOnGridCell(cellIndex) || IsBaseOnGridCell(cellIndex);
    }

    public bool IsTowerOnGridCell(Vector3Int cellIndex)
    {
        foreach (var tower in towers)
        {
            var worldPosition = grid.CellToWorld(cellIndex);
            if (tower.IsBuildingOnGridCell(worldPosition))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsBaseOnGridCell(Vector3Int cellIndex)
    {
        var worldPosition = grid.CellToWorld(cellIndex);
        return baseBuilding.IsBuildingOnGridCell(worldPosition);
    }

    public void RemoveTower(Vector3Int cellIndex)
    {
        foreach (var tower in towers)
        {
            if (tower.IsBuildingOnGridCell(cellIndex))
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
            // set sprite for gridCell
            AddTower(currentCell);
        }
        else if (IsTowerOnGridCell(currentCell))
        {
            RemoveTower(currentCell);
        }
    }
}
