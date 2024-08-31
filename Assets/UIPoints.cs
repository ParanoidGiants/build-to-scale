using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPoints : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    public void SetPoints(int points)
    {
        _textMesh.text = $"{points}P";
    }
}
