using UnityEngine;

public class KiemSi : MonoBehaviour
{
    private CheckHit checker;

    [Header("Charater Roll")]
    [SerializeField] private bool defender = false;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackAngle;

    private CharacterAnimation characterAnimation;
    public float speed = 2f;
    public float attackDamage = 10f;

    private void Awake()
    {
        checker = GetComponent<CheckHit>();
        characterAnimation = GetComponent<CharacterAnimation>(); 
        characterAnimation.OnAnimationEvent += HandleAnimationEvent;
    }

    private void Update()
    {
        if (characterAnimation.GetCurrentState() == CharacterState.Die || 
            characterAnimation.GetCurrentState() == CharacterState.TakeDamage) return;

        bool hasTarget = checker.CheckCircle(attackPos, attackAngle);

        if (hasTarget && defender)
        {
            Attack();
        }
        if (!defender)
        {
            if (!hasTarget)
            {
                MoveForward();
            }
            else
            {
                Attack();
            }
        }
    }

    private void MoveForward()
    {
        characterAnimation.SetState(CharacterState.Run);
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void Attack()
    {
        characterAnimation.SetState(CharacterState.Attack);
    }

    private void HandleAnimationEvent(string eventName)
    {
        if (eventName == CONSTANT.kiemSiAttack)
        {
            foreach (var enemy in checker.GetHits())
            {
                if(enemy.GetComponentInParent<CharacterHealth>() != null)
                {
                    enemy.GetComponentInParent<CharacterHealth>().TakeDamage(attackDamage);
                }
                else
                {
                    enemy.GetComponentInParent<StructuresHealth>().TakeDamage(attackDamage);
                }
            }
        }
    }

    private void OnDestroy()
    {
        characterAnimation.OnAnimationEvent -= HandleAnimationEvent;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackAngle);
    }
}