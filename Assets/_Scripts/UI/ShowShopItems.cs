using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowShopItems : MonoBehaviour
{
    [SerializeField] private CharacterSO[] characterSO;
    [SerializeField] private BuffSO[] buffSO;
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private BuyProgress buyProgress;

    [Header("Panel")]
    [SerializeField] private GameObject itemsBuffListHolder;
    [SerializeField] private GameObject shardShop;
    [SerializeField] private GameObject itemsShardListHolder;

    [Header("Prefabs")]
    [SerializeField] private GameObject itemsPrefab;

    [Header("Button")]
    [SerializeField] private Button buffShopButton;
    [SerializeField] private Button shardShopButton;
    [SerializeField] private Button refreshButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI refreshTime;

    private List<GameObject> shardItems = new List<GameObject>();
    private const string LAST_REFRESH_TIME = "LAST_REFRESH_TIME";
    private const string CHAR_SHARD_KEY = "CHAR_SHARD_";
    private const string CHAR_BOUGHT_KEY = "CHAR_BOUGHT_";

    private void Start()
    {
        LoadBuffShopItems();
        LoadChracterShardItems();

        CheckShopRefresh();

        buffShopButton.onClick.AddListener(ShowBuffShopItems);
        shardShopButton.onClick.AddListener(ShowChracterSharpItems);
        refreshButton.onClick.AddListener(() =>
        {
            buyProgress.StartBuyProcess();
            RefreshShardShop();
        });
        ShowBuffShopItems();
    }

    private void Update()
    {
        UpdateRefreshTimerUI();
    }

    private void ShowBuffShopItems()
    {
        itemsBuffListHolder.SetActive(true);
        shardShop.SetActive(false);
        buffManager.HideBuffDetailsPanel();
    }

    private void ShowChracterSharpItems()
    {
        itemsBuffListHolder.SetActive(false);
        shardShop.SetActive(true);
        buffManager.HideBuffDetailsPanel();
    }

    private void LoadBuffShopItems()
    {
        for (int i = 0; i < buffSO.Length; i++)
        {
            int index = i;
            GameObject buffButton = Instantiate(itemsPrefab, itemsBuffListHolder.transform);
            ItemsShopUI ui = buffButton.GetComponent<ItemsShopUI>();
            ui.characterSharpAmount.text = "";
            ui.itemsPrice.text = buffSO[i].Price.ToString();
            Button button = buffButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    buffManager.SetBuffSO(buffSO[index]);
                    buffManager.ShowBuffDetailPanel();
                });
            }
        }
    }

    private void LoadChracterShardItems()
    {
        for (int i = 0; i < characterSO.Length; i++)
        {
            int index = i;
            GameObject characterButton = Instantiate(itemsPrefab, itemsShardListHolder.transform);
            GameObject characterAvatar = Instantiate(characterSO[i].AvatarUI, characterButton.transform);
            characterAvatar.transform.SetSiblingIndex(0);
            ItemsShopUI ui = characterButton.GetComponent<ItemsShopUI>();

            string shardKey = CHAR_SHARD_KEY + index;
            string boughtKey = CHAR_BOUGHT_KEY + index;

            int number;

            if (!PlayerPrefs.HasKey(shardKey))
            {
                number = UnityEngine.Random.Range(5, 15);
                PlayerPrefs.SetInt(shardKey, number);
            }
            else
            {
                number = PlayerPrefs.GetInt(shardKey);
            }

            ui.characterSharpAmount.text = number.ToString();
            ui.itemsPrice.text = characterSO[i].Price.ToString();

            Button button = characterButton.GetComponent<Button>();

            bool isBought = PlayerPrefs.GetInt(boughtKey, 0) == 1;

            button.interactable = !isBought;

            shardItems.Add(characterButton);

            if (button != null)
            {
                button.onClick.AddListener(() => {
                    buyProgress.StartBuyProcess();
                    if (buyProgress.canBuy)
                    {
                        var prog = SaveManager.LoadCharacter(characterSO[index].ID) ?? characterSO[index].CreateDefaultProgress();
                        prog.shards += number;
                        SaveManager.SaveCharacter(prog);

                        button.interactable = false;
                        PlayerPrefs.SetInt(boughtKey, 1);
                        PlayerPrefs.Save();
                    }
                });
            }
        }
    }

    private void RefreshShardShop()
    {
        for (int i = 0; i < shardItems.Count; i++)
        {
            GameObject character = shardItems[i];

            ItemsShopUI ui = character.GetComponent<ItemsShopUI>();

            int newShard = UnityEngine.Random.Range(5, 15);

            PlayerPrefs.SetInt(CHAR_SHARD_KEY + i, newShard);

            PlayerPrefs.SetInt(CHAR_BOUGHT_KEY + i, 0);

            ui.characterSharpAmount.text = newShard.ToString();

            Button button = character.GetComponent<Button>();
            button.interactable = true;
        }

        PlayerPrefs.Save();
    }

    private void CheckShopRefresh()
    {
        string savedTime = PlayerPrefs.GetString(LAST_REFRESH_TIME, "");

        if (string.IsNullOrEmpty(savedTime))
        {
            SaveCurrentTime();
            return;
        }

        System.DateTime lastRefresh =
            System.DateTime.Parse(savedTime);

        System.TimeSpan timePassed =
            System.DateTime.Now - lastRefresh;

        if (timePassed.TotalHours >= 24)
        {
            RefreshShardShop();
            SaveCurrentTime();
        }
    }

    private void SaveCurrentTime()
    {
        PlayerPrefs.SetString(
            LAST_REFRESH_TIME,
            System.DateTime.Now.ToString()
        );

        PlayerPrefs.Save();
    }

    private void UpdateRefreshTimerUI()
    {
        string savedTime = PlayerPrefs.GetString(LAST_REFRESH_TIME, "");

        if (string.IsNullOrEmpty(savedTime))
        {
            refreshTime.text = "24:00:00";
            return;
        }

        System.DateTime lastRefresh =
            System.DateTime.Parse(savedTime);

        System.DateTime nextRefresh =
            lastRefresh.AddHours(24);

        System.TimeSpan remainingTime =
            nextRefresh - System.DateTime.Now;

        if (remainingTime.TotalSeconds <= 0)
        {
            refreshTime.text = "Refreshing...";

            CheckShopRefresh();

            return;
        }

        refreshTime.text =
            string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                remainingTime.Hours,
                remainingTime.Minutes,
                remainingTime.Seconds
            );
    }
}
