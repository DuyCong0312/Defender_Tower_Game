using UnityEngine;

public class ResourceManager
{
    private const string GOLD_KEY = "RES_GOLD";

    // ===== ADD =====
    public void AddGold(int amount)
    {
        int current = PlayerPrefs.GetInt(GOLD_KEY, 0);
        PlayerPrefs.SetInt(GOLD_KEY, current + amount);
    }

    // ===== SPEND =====
    public bool SpendGold(int amount)
    {
        int current = PlayerPrefs.GetInt(GOLD_KEY, 0);
        if (current >= amount)
        {
            PlayerPrefs.SetInt(GOLD_KEY, current - amount);
            return true;
        }
        return false;
    }

    // ===== GET =====
    public int GetGold()
    {
        return PlayerPrefs.GetInt(GOLD_KEY, 0);
    }

    // (Optional) reset
    public void ResetAll()
    {
        PlayerPrefs.DeleteKey(GOLD_KEY);
    }
}