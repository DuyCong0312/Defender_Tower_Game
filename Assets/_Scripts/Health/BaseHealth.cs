using System;
using UnityEngine;

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

        var characterSO = placeableSO as CharacterSO;
        if (characterSO != null)
        {
            var prog = SaveManager.LoadCharacter(characterSO.ID);
            if (prog != null)
            {
                maxHealth = prog.currentHealth;
            }
            else
            {
                maxHealth = placeableSO.healthAmount;
            }
        }
        else
        {
            maxHealth = placeableSO.healthAmount;
        }
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
    }

    protected virtual void OnDestroy()
    {
        return;
    }
}
