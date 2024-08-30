using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _cam;
    public float zoomSpeed = 1f;
    public float moveSpeed = 1f;
    private Vector3 fixPosition;
    
    void Start()
    {
        _cam = Camera.main;
    }

     private void LateUpdate()
    {
        var zoomDelta = -Input.mouseScrollDelta.y;
        _cam.orthographicSize += zoomDelta * zoomSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(1))
        {
            fixPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            var worldCurrentPosition = transform.position;
            worldCurrentPosition.z = 0;
            var delta =  fixPosition - worldCurrentPosition;
            transform.position = fixPosition + delta;
            fixPosition = Input.mousePosition;
        }
        
    }
}
