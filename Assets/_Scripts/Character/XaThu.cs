using UnityEngine;

public class XaThu : BaseCharacter
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPos;

    protected override void HandleAnimationEvent(string eventName)
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
}
