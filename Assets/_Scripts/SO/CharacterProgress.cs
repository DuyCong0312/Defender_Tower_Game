using System;

[Serializable]
public class CharacterProgress
{
    public int id;
    public int level;
    public int shards;
    public int currentHealth;
    public float attackDamage;

    public CharacterProgress() { }

    public CharacterProgress(int id, int level, int shards, int currentHealth, float attackDamage)
    {
        this.id = id;
        this.level = level;
        this.shards = shards;
        this.currentHealth = currentHealth;
        this.attackDamage = attackDamage;
    }
}
