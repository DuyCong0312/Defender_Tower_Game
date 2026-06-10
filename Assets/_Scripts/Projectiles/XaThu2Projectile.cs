using UnityEngine;

public class XaThu2Projectile : Projectile
{
    private int pierceCount = 1;
    private float damageReducePercent = 0.5f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(CONSTANT.Wall) ||
            collision.gameObject.CompareTag(CONSTANT.Character)) return;

        if (collision.gameObject.CompareTag(CONSTANT.Enemy))
        {
            BaseHealth enemyHealth = collision.GetComponentInParent<BaseHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);

                attackDamage *= (1f - damageReducePercent);

                pierceCount--;

                if (pierceCount < 0)
                {
                    Collider2D col = GetComponent<Collider2D>();
                    col.enabled = false;
                    Destroy(gameObject);
                }
            }

            Debug.Log(collision.name);
        }
    }

}
