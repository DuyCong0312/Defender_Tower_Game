using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float maxHealth;
    private CharacterAnimation characterAnimation;
    private float currentHealth;
    private bool isDead = false;
    private float leftEdge;

    void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).x - 0.5f;
        characterAnimation = GetComponent<CharacterAnimation>();
        characterAnimation.OnAnimationComplete += HandleAnimationEvent;

        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Update()
    {
        CheckBounds();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

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
            GameManager.Instance.IncreaseCoinAmount(100f);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        characterAnimation.OnAnimationEvent -= HandleAnimationEvent;
    }

    private void CheckBounds()
    {
        if (transform.position.x < leftEdge)
        {
            GameManager.Instance.Lose();
        }
    }
}