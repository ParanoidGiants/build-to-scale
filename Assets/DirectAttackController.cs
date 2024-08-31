using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DirectAttackController : MonoBehaviour
{
    public int radius = 1;
    public int damage = 1;
    public GameObject hitCircle;
    private Coroutine hitCircleCoroutine;
    private UIPoints _uiPoints;
    public int points = 0;

    private void Awake()
    {
        _uiPoints = FindObjectOfType<UIPoints>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var closestEnemy = WaveManager.Instance.GetClosestEnemy(worldPosition, radius);
        if (closestEnemy != null)
        {
            var dealtDamage = closestEnemy.Hit(damage);
            points += dealtDamage;
            _uiPoints.SetPoints(points);
        }

        // Animate Attack
        if (hitCircleCoroutine != null)
        {
            StopCoroutine(hitCircleCoroutine);
        }
        hitCircleCoroutine = StartCoroutine(SpawnHitCircle(worldPosition));
    }

    public IEnumerator SpawnHitCircle(Vector2 worldPosition)
    {
        var spawnCircleTimer = 1f;
        hitCircle.transform.position = worldPosition;
        hitCircle.SetActive(true);
        var spriteRenderer = hitCircle.GetComponent<SpriteRenderer>();
        while (spawnCircleTimer > 0f)
        {
            spawnCircleTimer -= Time.deltaTime;
            var color = spriteRenderer.color;
            color.a = spawnCircleTimer * spawnCircleTimer * spawnCircleTimer * spawnCircleTimer;
            spriteRenderer.color = color;
            yield return null;
        }
        hitCircle.SetActive(false);
    }
}
