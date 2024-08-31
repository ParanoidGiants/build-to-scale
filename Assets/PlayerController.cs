using System.Collections;
using System.Collections.Generic;
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
    public void SetAttackMode()
    {
        buildController.gameObject.SetActive(false);
        directAttackController.gameObject.SetActive(true);
    }

    public void EnterBuildMode()
    {
        buildController.gameObject.SetActive(true);
        directAttackController.gameObject.SetActive(false);
        mode = PlayerMode.BUILD;
    }

    public void ExitBuildMode()
    {
        buildController.gameObject.SetActive(false);
        directAttackController.gameObject.SetActive(true);
        mode = PlayerMode.ATTACK;
    }
}
