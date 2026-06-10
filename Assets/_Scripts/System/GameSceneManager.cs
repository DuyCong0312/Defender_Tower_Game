using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void SceneContinue()
    {
        Time.timeScale = 1.0f;
    }

    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneContinue();
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneContinue();
    }
}
