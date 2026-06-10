using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; 
    private const string UnlockedLevelKey = "UnlockedLevel";

    void OnEnable()
    {
        RefreshButtons();
    }

    public void OnLevelButtonClicked(int level)
    {
        SceneManager.LoadScene("Level" + level.ToString("D2"));
    }

    private void RefreshButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt(UnlockedLevelKey, 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i + 1) <= unlockedLevel;
        }
    }
}