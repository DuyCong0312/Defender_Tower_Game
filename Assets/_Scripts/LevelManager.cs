using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDatabase levelDatabase;

    private LevelSO currentLevel;
    private const string UnlockedLevelKey = "UnlockedLevel";

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
            if (l.sceneName == currentScene)
            {
                return l;
            }
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
        if (next > GetUnlockedLevel() && next <= totalLevels)
        {
            PlayerPrefs.SetInt(UnlockedLevelKey, next);
            PlayerPrefs.Save();
        }
    }

    public int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(UnlockedLevelKey, 1);
    }
}
