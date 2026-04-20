using UnityEngine;
using UnityEngine.Playables;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float timeExist = 2f;
    [SerializeField] private float speed = 10f;
    private GameObject owner;

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ProjectileMove();
    }

    private void Update()
    {
        timeExist -= Time.deltaTime;
        WayToDestroy();
    }

    private void ProjectileMove()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        if (owner.GetComponent<XaThu>().defender)
        {
            if (collision.gameObject.CompareTag(CONSTANT.Wall)) return;
        }

        if (collision.gameObject.CompareTag(CONSTANT.Enemy))
        {
            EnemyHealth playerHealth = collision.GetComponentInParent<EnemyHealth>();
            playerHealth.TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }

        Debug.Log(collision.name);
    }

    private void WayToDestroy()
    {
        if (timeExist <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
