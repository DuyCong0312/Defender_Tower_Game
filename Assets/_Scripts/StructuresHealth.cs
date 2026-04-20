using UnityEngine;

public class StructuresHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth;
    private PlacedObject placedObject;
    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        placedObject = GetComponent<PlacedObject>();

        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            isDead = true;
        }

        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void Die()
    {
        placedObject.RemoveFromGrid();
        Destroy(gameObject);
    }
}
