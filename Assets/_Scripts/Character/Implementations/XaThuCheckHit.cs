using UnityEngine;

public class XaThuCheckHit : CheckHit
{
    [SerializeField] private Transform attackPos;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private float attackAngle;

    public override void CheckEnemy()
    {
        hasTarget = CheckForwardBox(attackPos, attackSize, attackAngle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackSize);
    }
}
