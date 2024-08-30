using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Transform _player;
    private Camera _cam;
    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;
    
    void Start()
    {
        _cam = Camera.main;
        _player = _cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var xInput = Input.GetAxisRaw("Horizontal");
        var yInput = Input.GetAxisRaw("Vertical");
        var moveTowards = moveSpeed * Time.deltaTime * (transform.forward * yInput + transform.right * xInput);
        transform.position += moveTowards;

        var rotateDelta = rotateSpeed * Time.deltaTime * new Vector3(-Input.mouseScrollDelta.y, Input.mouseScrollDelta.x, 0f);
        Debug.Log(rotateDelta);
        transform.Rotate(rotateDelta);
    }
}
