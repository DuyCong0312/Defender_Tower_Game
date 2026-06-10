using UnityEngine;

public class KiemSi3 : BaseCharacter
{
    [SerializeField] private float ReduceSkillTime = 1f;
    private float timeToUseSkill = 0f;

    protected override void Update()
    {
        timeToUseSkill += Time.deltaTime;
        if (isBusy) return;
        if (!canMove) return;

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
                if(timeToUseSkill >= ReduceSkillTime)
                {
                    Skill();
                    timeToUseSkill = 0f;
                }
                else
                {
                    Attack();
                }
            }
        }
    }

    private void Skill()
    {
        isBusy = true;
        characterAnimation.SetState(CharacterState.Skill);
    }

    protected override void HandleAnimationEvent(string eventName)
    {
        if (eventName == CONSTANT.kiemSiAttack)
        {
            if(characterAnimation.GetCurrentState() == CharacterState.Attack)    
            {
                foreach (var enemy in checkHit.GetHits())
                {
                    BaseHealth enemyHealth = enemy.GetComponentInParent<BaseHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(runtimeAttackDamage);
                    }
                }
            }
            else if (characterAnimation.GetCurrentState() == CharacterState.Skill)
            {
                foreach (var enemy in checkHit.GetHits())
                {
                    BaseHealth enemyHealth = enemy.GetComponentInParent<BaseHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(runtimeAttackDamage * 1.5f);
                    }
                }
            }
        }
        
    }
}
