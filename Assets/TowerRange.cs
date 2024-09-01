using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRange : MonoBehaviour
{
    public TowerBuilding tower;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }
        tower.AddEnemy(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }
        tower.RemoveEnemy(enemy);
    }
}
