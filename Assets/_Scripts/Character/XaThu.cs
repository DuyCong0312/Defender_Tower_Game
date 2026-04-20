using UnityEngine;

public class XaThu : MonoBehaviour
{
    private CheckHit checker;  

    [Header("Charater Roll")]
    public bool defender = true;

    [SerializeField] private Transform attackPos;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private float attackAngle;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPos;

    private CharacterAnimation characterAnimation;
    public float speed = 2f;

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

        bool hasTarget = checker.CheckForwardBox(attackPos, attackSize, attackAngle);

        if (hasTarget && defender)
        {
            Attack();
        }
        else if(!defender) 
        {
            MoveForward();
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
        if (eventName == CONSTANT.xaThuAttack)
        {
            GameObject prefabs = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
            Projectile projectiles = prefabs.GetComponent<Projectile>();
            if(projectiles != null)
            {
                projectiles.SetOwner(this.gameObject);
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
        Gizmos.DrawWireCube(attackPos.position, attackSize);
    }
}
