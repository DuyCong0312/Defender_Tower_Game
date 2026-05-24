using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailUI : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Stat and Skill")]
    [SerializeField] private TextMeshProUGUI characterHealth;
    [SerializeField] private TextMeshProUGUI characterAtk;
    [SerializeField] private TextMeshProUGUI characterAtkSpeed;

    [Header("Progression")]
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button levelUpButton; 
    [SerializeField] private Color availableColor;
    [SerializeField] private Color unavailableColor;

    private CharacterSO characterSO;
    private GameObject currentCharacterPreview;

    private void Start()
    {
        levelUpButton.onClick.AddListener(() => {
            LevelUp();
            ShowAllDetail();
        });
    }

    public void SetCharacterSO(CharacterSO characterSO)
    {
        this.characterSO = characterSO;
    }

    private bool CheckHasCharacterSO()
    {
        return characterSO != null;
    }

    public void ShowAllDetail()
    {
        if (CheckHasCharacterSO())
        {
            GetAllCharacterStat();
            ShowCharacter();
            ShowCharacterName();
            ShowCharacterStat();
            ShowProgressSlider(); 
            UpdateLevelUpButtonState();
        }
    }

    private void GetAllCharacterStat()
    {
        characterSO.GetProgressData(
            characterSO.DisplayName + "Level",
            characterSO.DisplayName + "Shard");
        characterSO.GetStatData(
            characterSO.DisplayName + "Attack",
            characterSO.DisplayName + "Health");
    }

    private void ShowCharacter()
    {
        if(currentCharacterPreview != null)
        {
            Destroy(currentCharacterPreview);
        }
        currentCharacterPreview = Instantiate(characterSO.Avatar, spawnPos.position, spawnPos.rotation);
        currentCharacterPreview.transform.localScale = new Vector3(1.65f, 1.65f, 1f);
    }

    private void ShowCharacterName()
    {
        characterName.text = characterSO.DisplayName + "(LV." + characterSO.Level +")";
    }

    private void ShowCharacterStat()
    {
        characterHealth.text = characterSO.healthAmount.ToString();
        characterAtk.text = characterSO.AttackDamage.ToString();
        characterAtkSpeed.text = characterSO.AttackSpeed.ToString();
    }

    private bool ShowProgressSlider()
    {
        if (characterSO.Level == 0)
        {
            progressSlider.value = 1f;
        }
        else
        {
            progressSlider.value = (float)characterSO.CharacterShard / (characterSO.Level * 10);
        }
        progressText.text = characterSO.CharacterShard.ToString() + "/" + (characterSO.Level * 10).ToString();
        return progressSlider.value >= 1;
    }

    private void UpdateLevelUpButtonState()
    {
        bool canLevelUp = ShowProgressSlider();

        levelUpButton.image.color = canLevelUp
            ? availableColor
            : unavailableColor;
    }

    private void LevelUp()
    {
        int requireShard = characterSO.Level * 10;

        if (characterSO.Level == 0)
            requireShard = 0;

        if (characterSO.CharacterShard >= requireShard)
        {
            characterSO.MinusCharacterShard(requireShard, characterSO.DisplayName + "Shard");
            characterSO.LevelUp(characterSO.DisplayName + "Level"); 
            characterSO.UpgradeStats(characterSO.DisplayName + "Attack", characterSO.DisplayName + "Health");
        }
    }
}
