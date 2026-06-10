using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected CheckHit checkHit;
    protected CharacterAnimation characterAnimation;

    [Header("Charater Role")]
    public bool defender = false;
    [SerializeField] protected CharacterSO characterSO;
    [SerializeField] protected bool canMove = true;

    protected bool isBusy = false;

    protected float runtimeAttackDamage;
    protected float runtimeMovementSpeed;

    protected virtual void Awake()
    {
        checkHit = GetComponent<CheckHit>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }
    protected virtual void OnEnable()
    {
        if (characterSO != null)
        {
            var prog = SaveManager.LoadCharacter(characterSO.ID);
            runtimeAttackDamage = prog != null ? prog.attackDamage : characterSO.AttackDamage;
            runtimeMovementSpeed = characterSO.MovementSpeed;
        }
        else
        {
            runtimeAttackDamage = 0f;
            runtimeMovementSpeed = 0f;
        }

        characterAnimation.OnAnimationEvent += HandleAnimationEvent;
        characterAnimation.OnAnimationComplete += HandleAnimationComplete;
    }

    protected virtual void Update()
    {
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
                Attack();
            }
        }
    }

    public virtual void SetActiveCharacter(bool active)
    {
        canMove = active;
    }

    protected virtual void Move()
    {
        characterAnimation.SetState(CharacterState.Run);
        transform.Translate(Vector2.right * runtimeMovementSpeed * Time.deltaTime);
    }

    protected virtual void Attack()
    {
        isBusy = true;
        characterAnimation.SetState(CharacterState.Attack);
    }

    protected virtual void HandleAnimationEvent(string eventName)
    {
        if (eventName == CONSTANT.kiemSiAttack)
        {
            foreach (var enemy in checkHit.GetHits())
            {
                if (enemy == null) continue;

                BaseHealth enemyHealth = enemy.GetComponentInParent<BaseHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(runtimeAttackDamage);
                }
            }
        }
    }

    protected virtual void OnDestroy()
    {
        characterAnimation.OnAnimationEvent -= HandleAnimationEvent;
        characterAnimation.OnAnimationComplete -= HandleAnimationComplete;
    }

    protected virtual void HandleAnimationComplete(string animName)
    {
        if (animName == CONSTANT.attackAnimation ||
            animName == CONSTANT.skill1Animation ||
            animName == CONSTANT.takeDamageAnimation)
        {
            isBusy = false;
        }
    }

    public virtual CharacterSO GetCharacterSO()
    {
        return characterSO;
    }
}
