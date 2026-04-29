using Spine.Unity;
using System;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public event Action<GameObject> OnDeath;
    protected float leftEdge;

    protected CheckHit checkHit;
    protected CharacterAnimation characterAnimation;

    protected override void Awake()
    {
        base.Awake();

        checkHit = GetComponent<CheckHit>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    protected override void Start()
    {
        base.Start();

        leftEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).x - 0.5f;

        if (characterAnimation != null)
        {
            characterAnimation.OnAnimationComplete += HandleAnimationEvent;
        }
    }

    private void Update()
    {
        CheckBounds();
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (checkHit != null && !checkHit.hasTarget)
        {
            characterAnimation.SetState(CharacterState.TakeDamage);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            coll.enabled = false;
            Die();
        }

        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    protected override void Die()
    {
        characterAnimation.SetState(CharacterState.Die);
    }

    protected override void HandleAnimationEvent(string animName)
    {
        if (animName == CONSTANT.dieAnimation)
        {
            OnDeath?.Invoke(gameObject);
            GameManager.Instance.IncreaseCoinAmount(100f);
            Destroy(gameObject);
        }
    }

    protected override void OnDestroy()
    {
        characterAnimation.OnAnimationComplete -= HandleAnimationEvent;
    }

    private void CheckBounds()
    {
        if (transform.position.x < leftEdge)
        {
            GameManager.Instance.Lose();
        }
    }
}