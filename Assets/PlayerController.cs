using UnityEngine;

public enum PlayerMode
{
    BUILD,
    ATTACK
}

public class PlayerController : MonoBehaviour
{
    public PlayerMode mode = PlayerMode.ATTACK;
    private BuildController buildController;
    private DirectAttackController directAttackController;
    private void Awake()
    {
        directAttackController = GetComponentInChildren<DirectAttackController>();
        buildController = GetComponentInChildren<BuildController>();
    }

    private void Start()
    {
        ExitBuildMode();
    }

    public void EnterBuildMode()
    {
        buildController.gameObject.SetActive(true);
        directAttackController.gameObject.SetActive(false);
        mode = PlayerMode.BUILD;
        BuildingManager.Instance.ShowPlacementGrid();
    }

    public void ExitBuildMode()
    {
        buildController.gameObject.SetActive(false);
        directAttackController.gameObject.SetActive(true);
        mode = PlayerMode.ATTACK;
        BuildingManager.Instance.HidePlacementGrid();
    }
}
