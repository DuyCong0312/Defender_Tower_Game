using System.Collections.Generic;
using UnityEngine;

public class CheckHit : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemies;
    protected Collider2D coll;
    protected List<Collider2D> hitResults = new List<Collider2D>();
    public bool hasTarget;

    protected virtual void Start()
    {
        coll = GetComponentInChildren<Collider2D>();
    }

    protected virtual bool CheckForwardBox(Transform attackPos, Vector2 attackSize, float angle)
    {
        hitResults.Clear();

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos.position, attackSize, angle, whatIsEnemies);

        foreach (Collider2D enemy in hits)
        {
            if (enemy == coll) continue;

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

            hitResults.Add(enemy);
        }

        return hitResults.Count > 0;
    }

    public virtual List<Collider2D> GetHits()
    {
        return hitResults;
    }

    public virtual void CheckEnemy()
    {
        return;
    }
}