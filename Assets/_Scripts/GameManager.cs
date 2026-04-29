using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    public float coinAmount = 100f;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (DebugData.coin > 0)
        {
            coinAmount = DebugData.coin;
        }

        UpdateText();
    }

    public void Win()
    {
        panel.SetActive(true);
        winText.text = "You Win";
    }

    public void Lose()
    {
        panel.SetActive(true);
        winText.text = "You Lose";
    }

    public void IncreaseCoinAmount(float coinAmount)
    {
        this.coinAmount += coinAmount;
        UpdateText();
    }

    public void DecreaseCoinAmount(float price)
    {
        this.coinAmount -= price;
        UpdateText();
    }

    private void UpdateText()
    {
        coinAmountText.text = coinAmount.ToString();
    }


    // Test
    public void SetCoin(float coinAmount)
    {
        this.coinAmount = coinAmount;
        UpdateText();
    }
}
