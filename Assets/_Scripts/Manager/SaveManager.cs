using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    private const string SaveDataKey = "SaveManager_SaveData";

    [Serializable]
    private class SaveData
    {
        public int unlockedLevel = 1;
        public float discount = 0f;
        public int multipleShard = 1;
        public List<CharacterProgress> characters = new List<CharacterProgress>();
    }

    private static SaveData data;

    private static void EnsureLoaded()
    {
        if (data != null) return;

        try
        {
            var json = PlayerPrefs.GetString(SaveDataKey, string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            }
            else
            {
                data = new SaveData();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: failed to load save data from PlayerPrefs: {ex}");
            data = new SaveData();
        }
    }

    public static void PersistToDisk()
    {
        EnsureLoaded();

        try
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveDataKey, json);
            PlayerPrefs.Save();
            Debug.Log("SaveManager: saved data to PlayerPrefs.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: failed to persist save data to PlayerPrefs: {ex}");
        }
    }

    public static void ClearAll()
    {
        data = new SaveData();
        PlayerPrefs.DeleteKey(SaveDataKey);
        PlayerPrefs.Save();
        Debug.Log("SaveManager: cleared save data from PlayerPrefs.");
    }

    // Unlocked level
    public static int GetUnlockedLevel()
    {
        EnsureLoaded();
        return data.unlockedLevel;
    }

    public static void SetUnlockedLevel(int level)
    {
        EnsureLoaded();
        data.unlockedLevel = level;
    }

    // Discount & multiple shard
    public static float GetDiscount()
    {
        EnsureLoaded();
        return data.discount;
    }

    public static void SetDiscount(float value)
    {
        EnsureLoaded();
        data.discount = value;
    }

    public static int GetMultipleShard()
    {
        EnsureLoaded();
        return data.multipleShard;
    }

    public static void SetMultipleShard(int value)
    {
        EnsureLoaded();
        data.multipleShard = value;
    }

    // Characters
    public static CharacterProgress LoadCharacter(int id)
    {
        EnsureLoaded();
        return data.characters.Find(c => c.id == id);
    }

    public static void SaveCharacter(CharacterProgress progress)
    {
        EnsureLoaded();
        var existing = data.characters.Find(c => c.id == progress.id);
        if (existing != null) data.characters.Remove(existing);
        data.characters.Add(progress);
    }
}