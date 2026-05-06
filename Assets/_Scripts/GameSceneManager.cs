using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
}
