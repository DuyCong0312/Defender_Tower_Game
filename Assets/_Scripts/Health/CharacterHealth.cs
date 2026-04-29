using UnityEngine;
using UnityEngine.Playables;

public class CharacterHealth : BaseHealth
{
    protected CheckHit checkHit;
    protected CharacterAnimation characterAnimation;
    protected PlacedObject placedObject;

    protected override void Awake()
    {
        base.Awake();

        checkHit = GetComponent<CheckHit>();
        characterAnimation = GetComponent<CharacterAnimation>();
        placedObject = GetComponent<PlacedObject>();
    }

    protected override void Start()
    {
        base.Start();

        if (characterAnimation != null)
        {
            characterAnimation.OnAnimationComplete += HandleAnimationEvent;
        }
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
            placedObject.RemoveFromGrid();
            Destroy(gameObject);
        }
    }

    protected override void OnDestroy()
    {
        characterAnimation.OnAnimationComplete -= HandleAnimationEvent;
    }
}