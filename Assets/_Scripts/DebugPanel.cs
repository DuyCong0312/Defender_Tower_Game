using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField coinInput;
    [SerializeField] private TMP_InputField enemy0Input;
    [SerializeField] private TMP_InputField enemy1Input;
    [SerializeField] private TMP_InputField enemy2Input;
    [SerializeField] private TMP_InputField enemy3Input;

    [SerializeField] private Spawner spawner;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void SetCoin()
    {
        float coin = float.Parse(coinInput.text); 
        DebugData.coin = coin;
        GameManager.Instance.SetCoin(coin);
    }

    public void SetEnemy0()
    {
        int amount = int.Parse(enemy0Input.text);
        DebugData.enemy0 = amount;
        spawner.SetEnemyAmount(0, amount);
        spawner.Rebuild();
    }

    public void SetEnemy1()
    {
        int amount = int.Parse(enemy1Input.text);
        DebugData.enemy1 = amount;
        spawner.SetEnemyAmount(1, amount);
        spawner.Rebuild();
    }

    public void SetEnemy2()
    {
        int amount = int.Parse(enemy2Input.text);
        DebugData.enemy2 = amount;
        spawner.SetEnemyAmount(2, amount);
        spawner.Rebuild();
    }

    public void SetEnemy3()
    {
        int amount = int.Parse(enemy3Input.text);
        DebugData.enemy3 = amount;
        spawner.SetEnemyAmount(3, amount);
        spawner.Rebuild();
    }

    public void AddQuickCoins()
    {
        GameManager.Instance.IncreaseCoinAmount(999);
    }

    public void ResetSpawn()
    {
        spawner.Rebuild();
    }

    public void SaveAndRestart()
    {
        SetCoin();
        SetEnemy0();
        SetEnemy1();
        SetEnemy2();
        SetEnemy3();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}