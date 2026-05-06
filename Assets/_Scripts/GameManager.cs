using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float coinAmount = 100f;

    public float discount = 0f;
    private int award = 100;

    public static System.Action<float> OnCoinChanged;
    public static System.Action<int> OnGoldChanged;
    public static System.Action<int> OnWin;
    public static System.Action OnLose;

    private ResourceManager rm;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rm = new ResourceManager();

        discount = PlayerPrefs.GetFloat("Discount", 0f);

        OnCoinChanged?.Invoke(coinAmount);
        OnGoldChanged?.Invoke(rm.GetGold());
    }

    public void Win()
    {
        rm.AddGold(award);

        OnWin?.Invoke(award);
        OnGoldChanged?.Invoke(rm.GetGold());
    }

    public void Lose()
    {
        OnLose?.Invoke();
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
}