using UnityEngine;

public class KiemSiCheckHit : CheckHit
{
    [SerializeField] protected Transform attackPos;
    [SerializeField] protected float attackAngle;

    public override void CheckEnemy()
    {
        hasTarget = CheckCircle(attackPos, attackAngle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackAngle);
    }
}
