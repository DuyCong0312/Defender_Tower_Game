using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private TextMeshProUGUI awardText;

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnCoinChanged += UpdateCoin;
        GameManager.OnGoldChanged += UpdateGold;
        GameManager.OnWin += ShowWin;
        GameManager.OnLose += ShowLose;
    }

    private void OnDisable()
    {
        GameManager.OnCoinChanged -= UpdateCoin;
        GameManager.OnGoldChanged -= UpdateGold;
        GameManager.OnWin -= ShowWin;
        GameManager.OnLose -= ShowLose;
    }

    public void ShowWin(int award)
    {
        winPanel.SetActive(true);
        winText.text = "You Win";
        awardText.text = "Gold +" + award;
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        loseText.text = "You Lose";
    }

    public void UpdateCoin(float value)
    {
        coinAmountText.text = value.ToString();
    }

    public void UpdateGold(int gold)
    {
        resourceText.text = gold.ToString();
    }
}