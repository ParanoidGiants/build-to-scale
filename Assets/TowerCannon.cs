using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCannon : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void Shoot(Transform enemy)
    {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, null).GetComponent<ProjectileController>();
        var flightDirection = enemy.position - transform.position;
        projectile.Shoot(transform.position, flightDirection);
    }

    internal void UpdateRotation(Enemy closestEnemy)
    {
        var lookDirection = closestEnemy.transform.position - transform.position;
        transform.up = lookDirection;
    }
}
