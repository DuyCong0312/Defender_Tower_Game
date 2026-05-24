using UnityEngine;

public class StructuresHealth : BaseHealth
{
    protected BaseCharacter baseCharacter;
    protected PlacedObject placedObject;

    protected override void Awake()
    {
        base.Awake();

        placedObject = GetComponent<PlacedObject>();
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }

        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    protected override void Die()
    {
        healthBar.gameObject.SetActive(false);
        placedObject.RemoveFromGrid();
        Destroy(gameObject);
    }
}
