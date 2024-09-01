using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHitPoints : MonoBehaviour
{
    public Image bar;
    public TextMeshProUGUI textMesh;
    
    public void SetHitPoints(int currentHitPoints, int maxHitPoints)
    {
        textMesh.text = $"HP: {currentHitPoints} / {maxHitPoints}";
        var relation = (float) currentHitPoints / maxHitPoints;
        bar.fillAmount = relation;
    }
}
