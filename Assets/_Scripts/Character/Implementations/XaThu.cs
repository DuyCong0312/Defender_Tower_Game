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
                var prog = SaveManager.LoadCharacter(characterSO.ID);
                float damage = prog != null ? prog.attackDamage : characterSO.AttackDamage;
                projectiles.SetOwner(this.gameObject, damage);
            }
        }
    }
}
