using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDatabase levelDatabase;

    private LevelSO currentLevel;

    private void Awake()
    {
        currentLevel = GetCurrentLevel(levelDatabase);
    }

    private void Start()
    {
        UIManager.Instance.StartAnnoucement("Level" + currentLevel.levelNumber + " " + "Start");
    }

    public LevelSO GetCurrentLevel(LevelDatabase db)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (LevelSO l in db.levels)
        {
            if (l.sceneName == currentScene) return l;
        }
        return null;
    }

    public void UnlockNextLevel()
    {
        UnlockLevel(currentLevel.levelNumber, levelDatabase.TotalLevels);
    }

    public void UnlockLevel(int completedLevelNumber, int totalLevels)
    {
        int next = completedLevelNumber + 1;
        int unlocked = SaveManager.GetUnlockedLevel();
        if (next > unlocked && next <= totalLevels)
        {
            SaveManager.SetUnlockedLevel(next);
        }
    }

    public int GetUnlockedLevel()
    {
        return SaveManager.GetUnlockedLevel();
    }
}
