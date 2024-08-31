using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeButton : MonoBehaviour
{
    private PlayerController _playerController;
    private Image _buttonImage;
    private Button _button;
    private bool isInteractable;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI buildRemoveText;
    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _buttonImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        isInteractable = _button.interactable;
    }

    private void Update()
    {
        var maxBuildingCount = BuildingManager.Instance.maxPlacementCount;
        var remainingBuildingCount = maxBuildingCount - BuildingManager.Instance.currentPlacementCount;
        counterText.text = $"{remainingBuildingCount}/{maxBuildingCount}";

        if (remainingBuildingCount > 0 || maxBuildingCount == 0)
        {
            buildRemoveText.text = $"Build\nRemove";
        }
        else if (isInteractable) 
        {
            buildRemoveText.text = $"Remove";
        }

        if (!isInteractable && maxBuildingCount > 0)
        {
            _button.interactable = true;
            isInteractable = true;
        }
        else if (isInteractable && maxBuildingCount == 0)
        {
            _button.interactable = false;
            isInteractable = false;
        }
    }

    public void OnToggleBuildMode()
    {
        if (_playerController.mode != PlayerMode.BUILD)
        {
            _buttonImage.color = Color.green;
            _playerController.EnterBuildMode();
        }
        else
        {
            _buttonImage.color = Color.white;
            _playerController.ExitBuildMode();
        }
    }
}
