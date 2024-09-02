using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 1f;
    public float maxFlightDistance = 0f;
    public float coveredFlightDistance = 1f;
    public Vector2 startPosition = Vector2.zero;
    public Vector2 flightDirection = Vector2.up;
    public bool isFlying = false;
    public int damage = 1;


    void Update()
    {
        if (!isFlying)
        {
            return;
        }
        transform.position += (Vector3)flightDirection * speed * Time.deltaTime;
        coveredFlightDistance = Vector3.Distance(startPosition, transform.position);
        if (coveredFlightDistance >= maxFlightDistance)
        {
            Destroy(gameObject);
            isFlying = false;
        }
    }

    public void Shoot(Vector2 startPosition, Vector2 flightDirection)
    {
        this.startPosition = startPosition;
        this.flightDirection = flightDirection;
        isFlying = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Hit(damage);
            Destroy(gameObject);
        }
    }
}
