using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character")]
public class CharacterSO : PlaceableSO
{
    [Header("Character Stats")]
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float attackDamage = 10f;

    [Header("Progression base (template only)")]
    [SerializeField] private int baseLevel = 0;
    [SerializeField] private int baseCharacterShard = 0;

    public float AttackSpeed => attackSpeed;
    public float MovementSpeed => movementSpeed;
    public float AttackDamage => attackDamage;
    public int BaseLevel => baseLevel;
    public int BaseCharacterShard => baseCharacterShard;

    public float GetAttackDamageForLevel(int level)
    {
        return attackDamage * (1f + level * 0.1f);
    }

    public int GetHealthForLevel(int level)
    {
        return Mathf.RoundToInt(healthAmount * (1f + level * 0.25f));
    }

    public CharacterProgress CreateDefaultProgress()
    {
        return new CharacterProgress(ID, baseLevel, baseCharacterShard, GetHealthForLevel(baseLevel), GetAttackDamageForLevel(baseLevel));
    }
}