using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Spawner;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterSO[] characterSO;

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject annoucement;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private TextMeshProUGUI awardText;
    [SerializeField] private TextMeshProUGUI annoucementText;

    [Header("Award")]
    [SerializeField] private RectTransform CharacterAwardMenu;
    [SerializeField] private GameObject characterAward;

    [Header("ProgressEnemySpawn")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private RectTransform sliderArea;
    [SerializeField] private GameObject waveMarkerPrefab;
    private Spawner spawner;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spawner = FindFirstObjectByType<Spawner>();
        SpawnWaveMarkers();
    }

    private void OnEnable()
    {
        gameManager.OnCoinChanged += UpdateCoin;
        gameManager.OnGoldChanged += UpdateGold;
        gameManager.OnWin += ShowWin;
        gameManager.OnLose += ShowLose;
    }

    private void OnDisable()
    {
        gameManager.OnCoinChanged -= UpdateCoin;
        gameManager.OnGoldChanged -= UpdateGold;
        gameManager.OnWin -= ShowWin;
        gameManager.OnLose -= ShowLose;
    }

    public void ShowWin(int award)
    {
        winPanel.SetActive(true);
        winText.text = "You Win";
        awardText.text = "Gold +" + award;
        ShowAwardCharacterShard();
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        loseText.text = "You Lose";
    }

    public void ShowPause()
    {
        pausePanel.SetActive(true);
    }

    public void UpdateCoin(float value)
    {
        coinAmountText.text = value.ToString();
    }

    public void UpdateGold(int gold)
    {
        resourceText.text = gold.ToString();
    }

    public void StartAnnoucement(string message)
    {
        StartCoroutine(Annoucement(message));
    }

    private IEnumerator Annoucement(string message)
    {
        annoucement.SetActive(true);
        annoucementText.text = message;
        yield return new WaitForSeconds(1f);
        annoucement.SetActive(false);
    }

    private void ShowAwardCharacterShard()
    {
        for (int i = 0; i < characterSO.Length; i++)
        {
            int index = i;
            GameObject character = Instantiate(characterAward, CharacterAwardMenu);

            GameObject characterAvatar = Instantiate(characterSO[i].AvatarUI, character.transform);
            characterAvatar.transform.SetSiblingIndex(0);

            var shardAward = character.GetComponentInChildren<TMP_Text>();
            if (shardAward != null)
            {
                shardAward.text = "+" + gameManager.multipleShard.ToString();
            }
        }
    }

    public void UpdateProgressSlider()
    {
        if (spawner.totalEnemies > 0)
        {
            progressSlider.value = (float)spawner.spawnedEnemies / spawner.totalEnemies;
        }
    }

    public void SpawnWaveMarkers()
    {
        float width = sliderArea.rect.width;

        foreach (float point in spawner.waveMarkerPoints)
        {
            float xPos = (width / 2f) - (point * width);

            GameObject obj = Instantiate(waveMarkerPrefab, sliderArea);

            RectTransform rect = obj.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(xPos, 0);
        }
    }
}