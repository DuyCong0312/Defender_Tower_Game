using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected PlaceableSO placeableSO;

    protected Collider2D coll;

    protected float maxHealth;
    protected float currentHealth;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        coll = GetComponentInChildren<Collider2D>();
        if(PlayerPrefs.HasKey(placeableSO.DisplayName + "Health"))
        {
            placeableSO.healthAmount = PlayerPrefs.GetInt(placeableSO.DisplayName + "Health");
        }
        maxHealth = placeableSO.healthAmount;
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
        return;
    }

    protected virtual void Die()
    {
        return;
    }

    protected virtual void HandleAnimationEvent(string animName)
    {
        return;

        if (animName == CONSTANT.dieAnimation)
        {
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        return;
    }
}
