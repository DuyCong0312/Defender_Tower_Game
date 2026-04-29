using UnityEngine;

public class KiemSi3 : BaseCharacter
{
    [SerializeField] private float ReduceSkillTime = 1f;
    private float timeToUseSkill = 0f;

    protected override void Update()
    {
        timeToUseSkill += Time.deltaTime;
        if (isBusy) return;

        if (characterAnimation.GetCurrentState() == CharacterState.Die ||
            characterAnimation.GetCurrentState() == CharacterState.TakeDamage) return;

        Debug.Log(characterAnimation.GetCurrentState());
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
                        enemyHealth.TakeDamage(characterSO.AttackDamage);
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
                        enemyHealth.TakeDamage(characterSO.AttackDamage * 1.5f);
                    }
                }
            }
        }
        
    }
}
