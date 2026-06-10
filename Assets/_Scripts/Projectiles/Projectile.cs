using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float speed = 10f;
    protected GameObject owner;
    protected float attackDamage;

    protected Renderer sr;
    protected Camera cam;

    public virtual void SetOwner(GameObject owner, float damage)
    {
        this.owner = owner;
        this.attackDamage = damage;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<Renderer>();
        cam = Camera.main;
    }

    protected virtual void Start()
    {
        ProjectileMove();
    }

    protected virtual void Update()
    {
        WayToDestroy();
    }

    protected virtual void ProjectileMove()
    {
        rb.linearVelocity = transform.right * speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(CONSTANT.Wall) ||
            collision.gameObject.CompareTag(CONSTANT.Character)) return;

        if (collision.gameObject.CompareTag(CONSTANT.Enemy))
        {
            BaseHealth enemyHealth = collision.GetComponentInParent<BaseHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
            Destroy(this.gameObject); 
            return;
        }
    }

    protected virtual void WayToDestroy()
    {
        if (sr == null || cam == null) return;
        Bounds bounds = sr.bounds;
        Vector3 min = cam.WorldToViewportPoint(bounds.min);
        Vector3 max = cam.WorldToViewportPoint(bounds.max);
        if (max.x < 0 || min.x > 1 || max.y < 0 || min.y > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
