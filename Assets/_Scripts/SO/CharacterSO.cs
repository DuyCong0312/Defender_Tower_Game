using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character")]
public class CharacterSO : PlaceableSO
{
    [Header("Character Stats")]
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float attackDamage = 10f;

    public float AttackSpeed => attackSpeed;
    public float MovementSpeed => movementSpeed;
    public float AttackDamage => attackDamage;
}
