using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    private static int ENEMY_COUNT = 0;
    private int _id;
    public int Id { get; }
    public UnityEvent<int> died;
    public float moveSpeed = 1f;
    private BaseBuilding baseBuilding;
    public int hitPoints = 1;
    
    private void Awake()
    {
        _id = ENEMY_COUNT;
        ++ENEMY_COUNT;
    }

    void Start()
    {
        baseBuilding = FindObjectOfType<BaseBuilding>();
    }

    void Update()
    {
        var moveDirection = Vector3.Normalize(baseBuilding.transform.position - transform.position);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        died.Invoke(_id);
        died.RemoveAllListeners();
    }

    public int Hit(int damage)
    {
        var dealtDamage = Mathf.Min(damage, hitPoints);
        hitPoints -= damage;
        if (hitPoints < 0)
        {
            Destroy(gameObject);
        }
        return dealtDamage;
    }
}
