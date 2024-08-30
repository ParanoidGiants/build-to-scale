using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseBuilding : MonoBehaviour
{
    private Tilemap _tilemap;
    
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemap.CompressBounds();
    }

    
    public bool IsBuildingOnGridCell(Vector3 worldPosition)
    {
        var cellIndex = _tilemap.WorldToCell(worldPosition);
        Debug.Log($"BASE: {cellIndex}");
        return _tilemap.GetTile(cellIndex) != null;
    }
}
