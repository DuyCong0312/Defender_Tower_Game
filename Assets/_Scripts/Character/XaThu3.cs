using UnityEngine;

public class XaThu3 : BaseCharacter
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPos;

    protected override void Update()
    {
        if (characterAnimation.GetCurrentState() == CharacterState.Die ||
            characterAnimation.GetCurrentState() == CharacterState.TakeDamage) return;

        checkHit.CheckEnemy();

        if (checkHit.hasTarget && defender)
        {
            Attack();
        }
        if (!defender)
        {
            if (!checkHit.hasTarget)
            {
                Move();
            }
            else
            {
                Attack();
            }
        }
    }

    protected override void HandleAnimationEvent(string eventName)
    {
        if (eventName == CONSTANT.xaThuAttack)
        {
            Transform target = FindNearestEnemy();
            if (target == null) return;

            Vector2 direction = (target.position - spawnPos.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject prefabs = Instantiate(projectilePrefab, spawnPos.position, Quaternion.Euler(0, 0, angle));

            Projectile projectiles = prefabs.GetComponent<Projectile>();
            if (projectiles != null)
            {
                projectiles.SetOwner(this.gameObject);
            }
        }
    }

    private Transform FindNearestEnemy()
    {
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (var enemy in checkHit.GetHits())
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}
