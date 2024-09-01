using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class BaseBuilding : MonoBehaviour
{
    private Tilemap _tilemap;
    public int maxHitPoints = 1;
    private int _hitPoints;
    private UIHitPoints _uiHitPoints;
    
    private void Awake()
    {
        _uiHitPoints = FindObjectOfType<UIHitPoints>();
        _tilemap = GetComponent<Tilemap>();
        _hitPoints = maxHitPoints;
    }

    void Start()
    {
        _tilemap.CompressBounds();
    }
    
    public bool IsBuildingOnWorldPosition(Vector3 worldPosition)
    {
        var cellIndex = _tilemap.WorldToCell(worldPosition);
        return _tilemap.GetTile(cellIndex) != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        Hit(enemy.hitPoints);
        enemy.Hit(enemy.hitPoints);
    }
    
    public int Hit(int damage)
    {
        var dealtDamage = Mathf.Min(damage, _hitPoints);
        _hitPoints -= damage;
        _uiHitPoints.SetHitPoints(_hitPoints, maxHitPoints);
        if (_hitPoints <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        return dealtDamage;
    }
}
