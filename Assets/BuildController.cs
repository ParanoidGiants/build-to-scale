using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    public BuildingManager buildingManager;
    public Grid grid;
    public Vector3Int lastCell;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        var mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var currentCell = grid.WorldToCell(mousePositionWorld);

        if (Input.GetMouseButtonDown(0))
        {
            buildingManager.HandleClickOnCell(currentCell);
        }
        lastCell = currentCell;
        
    }
}
