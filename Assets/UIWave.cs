using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWave : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    public void SetTimeText(float time)
    {
        _textMesh.text = $"Next Wave: {time:0.0}s";
    }

    public void SetEnemiesAttackingText()
    {
        _textMesh.text = "!Enemies Attacking!";
    }
}
