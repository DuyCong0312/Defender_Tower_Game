using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float coinAmount = 100f;

    public float discount = 0f;
    public int multipleShard = 1;
    private int award = 100;

    public event Action<float> OnCoinChanged;
    public event Action<int> OnGoldChanged;
    public event Action<int> OnWin;
    public event Action OnLose;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private CharacterSO[] characterSO;

    private ResourceManager rm;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rm = new ResourceManager();

        discount = PlayerPrefs.GetFloat("Discount", 0f);
        multipleShard = PlayerPrefs.GetInt("MultipleShard", 1);

        OnCoinChanged?.Invoke(coinAmount);
        OnGoldChanged?.Invoke(rm.GetGold());
    }

    public void Win()
    {
        rm.AddGold(award);
        OnWin?.Invoke(award);
        OnGoldChanged?.Invoke(rm.GetGold());
        levelManager.UnlockNextLevel();
        foreach (CharacterSO character in characterSO)
        {
            character.AddCharacterShard(1 * multipleShard, character.DisplayName + "Shard");
        }
        ResetBuff();
    }

    public void Lose()
    {
        OnLose?.Invoke();
        ResetBuff();
    }

    public void IncreaseCoinAmount(float amount)
    {
        coinAmount += amount;
        OnCoinChanged?.Invoke(coinAmount);
    }

    public void DecreaseCoinAmount(float price)
    {
        coinAmount -= price * (1 - discount);
        OnCoinChanged?.Invoke(coinAmount);
    }

    public void SaveAndSetDiscount(float discount)
    {
        this.discount = discount;
        PlayerPrefs.SetFloat("Discount", discount);
        PlayerPrefs.Save();
    }

    public void SaveAndSetMultipleShard(int amount)
    {
        this.multipleShard = amount;
        PlayerPrefs.SetInt("MultipleShard", multipleShard);
        PlayerPrefs.Save();
    }

    // need refactor
    private void ResetBuff()
    {
        discount = 0f;
        multipleShard = 1;

        PlayerPrefs.DeleteKey("Discount");
        PlayerPrefs.DeleteKey("MultipleShard");

        PlayerPrefs.Save();
    }
}