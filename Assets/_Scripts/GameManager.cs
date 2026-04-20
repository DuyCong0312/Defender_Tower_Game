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
        UpdateText();
    }

    private void Update()
    {
        if(coinAmount >= 1000f)
        {
            panel.SetActive(true);
            winText.text = "You Win";
        }
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
}
