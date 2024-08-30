using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerBuilding : MonoBehaviour
{
    private Tilemap _tilemap;
    public GameObject range;
    public bool isRangeVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemap.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {
        range.SetActive(isRangeVisible);
    }
    
    public bool IsBuildingOnGridCell(Vector3 worldPosition)
    {
        var cellIndex = _tilemap.WorldToCell(worldPosition);
        Debug.Log($"TOWER: {cellIndex}");
        return _tilemap.GetTile(cellIndex) != null;
    }
}
