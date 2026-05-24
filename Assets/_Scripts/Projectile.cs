using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float timeExist = 2f;
    [SerializeField] private float speed = 10f;
    private GameObject owner;

    private Renderer sr;
    private Camera cam;

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<Renderer>();
        cam = Camera.main;
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

        if (owner.GetComponent<BaseCharacter>().defender)
        {
            if (collision.gameObject.CompareTag(CONSTANT.Wall) ||
                collision.gameObject.CompareTag(CONSTANT.Character)) return;
        }

        if (collision.gameObject.CompareTag(CONSTANT.Enemy))
        {
            BaseHealth enemyHealth = collision.GetComponentInParent<BaseHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(owner.GetComponent<BaseCharacter>().GetCharacterSO().AttackDamage);
            }
            Destroy(this.gameObject);
        }

        Debug.Log(collision.name);
    }

    private void WayToDestroy()
    {
        Bounds bounds = sr.bounds;
        Vector3 min = cam.WorldToViewportPoint(bounds.min);
        Vector3 max = cam.WorldToViewportPoint(bounds.max);
        if (max.x < 0 || min.x > 1 || max.y < 0 || min.y > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
