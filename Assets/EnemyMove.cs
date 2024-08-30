using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 1f;
    private BaseBuilding baseBuilding;
    // Start is called before the first frame update
    void Start()
    {
        baseBuilding = FindObjectOfType<BaseBuilding>();
    }

    // Update is called once per frame
    void Update()
    {
        var moveDirection = Vector3.Normalize(baseBuilding.transform.position - transform.position);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
