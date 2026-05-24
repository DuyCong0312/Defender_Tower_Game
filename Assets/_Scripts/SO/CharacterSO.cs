using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character")]
public class CharacterSO : PlaceableSO
{
    [Header("Character Stats")]
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float attackDamage = 10f;

    [Header("Progression")]
    [SerializeField] private int level = 0;
    [SerializeField] private int characterShard = 0;

    public float AttackSpeed => attackSpeed;
    public float MovementSpeed => movementSpeed;
    public float AttackDamage => attackDamage;
    public int Level => level;
    public int CharacterShard => characterShard;

    public void AddCharacterShard(int amount, string name)
    {
        characterShard += amount;
        SaveInt(name, characterShard);
    }

    public void MinusCharacterShard(int amount, string name)
    {
        characterShard -= amount;
        SaveInt(name, characterShard);
    }

    public void LevelUp(string name)
    {
        level++;
        SaveInt(name, level);
    }

    public void UpgradeStats(string damageKey, string healthKey)
    {
        attackDamage += attackDamage * (level * 0.1f);
        healthAmount = Mathf.RoundToInt(healthAmount + healthAmount * 0.25f);
        SaveFloat(damageKey, attackDamage);
        SaveInt(healthKey, healthAmount);
    }

    private void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public void GetProgressData(string levelKey, string shardKey)
    {
        level = PlayerPrefs.GetInt(levelKey, level);
        characterShard = PlayerPrefs.GetInt(shardKey, characterShard);
    }

    public void GetStatData(string damageKey, string healthKey)
    {
        attackDamage = PlayerPrefs.GetFloat(damageKey, attackDamage);
        healthAmount = PlayerPrefs.GetInt(healthKey, healthAmount);
    }
}