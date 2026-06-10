using System.Collections.Generic;
using UnityEngine;

public class CheckHit : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemies;
    protected Collider2D coll;
    protected List<Collider2D> hitResults = new List<Collider2D>();
    public bool hasTarget;
    private Camera cam;

    protected virtual void Start()
    {
        coll = GetComponentInChildren<Collider2D>();
        cam = Camera.main;
    }

    protected virtual bool CheckForwardBox(Transform attackPos, Vector2 attackSize, float angle)
    {
        hitResults.Clear();

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos.position, attackSize, angle, whatIsEnemies);

        foreach (Collider2D enemy in hits)
        {
            if (enemy == coll) continue;
            if (!IsEnemyInViewPort(enemy.transform.root.gameObject)) continue;

            hitResults.Add(enemy);
        }

        return hitResults.Count > 0;
    }

    protected virtual bool CheckCircle(Transform attackPos, float range)
    {
        hitResults.Clear();

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos.position, range, whatIsEnemies);

        foreach (Collider2D enemy in hits)
        {
            if (enemy == coll) continue;
            if (!IsEnemyInViewPort(enemy.transform.root.gameObject)) continue;

            hitResults.Add(enemy);
        }

        return hitResults.Count > 0;
    }

    protected virtual bool IsEnemyInViewPort(GameObject enemy) 
    {
        Renderer sr = enemy.GetComponentInChildren<Renderer>();
        if (sr == null) return false;

        Bounds bounds = sr.bounds;
        Vector3 min = cam.WorldToViewportPoint(bounds.min);
        Vector3 max = cam.WorldToViewportPoint(bounds.max);

        return min.x >= 0 && max.x <= 1 &&
               min.y >= 0 && max.y <= 1;
    }

    public virtual List<Collider2D> GetHits()
    {
        hitResults.RemoveAll(x => x == null);
        return hitResults;
    }

    public virtual void CheckEnemy()
    {
        return;
    }
}