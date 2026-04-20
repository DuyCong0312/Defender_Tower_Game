using UnityEngine;
using UnityEngine.Playables;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth;
    private CharacterAnimation characterAnimation;
    private PlacedObject placedObject;
    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        characterAnimation.OnAnimationComplete += HandleAnimationEvent;
        placedObject = GetComponent<PlacedObject>();

        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if(isDead) return;

        currentHealth -= damage;
        characterAnimation.SetState(CharacterState.TakeDamage);

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }

        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void Die()
    {
        characterAnimation.SetState(CharacterState.Die);
    }

    private void HandleAnimationEvent(string animName)
    {
        if (animName == CONSTANT.dieAnimation)
        {
            placedObject.RemoveFromGrid();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        characterAnimation.OnAnimationEvent -= HandleAnimationEvent;
    }
}