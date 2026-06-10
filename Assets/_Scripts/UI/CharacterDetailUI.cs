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
    private CharacterProgress currentProgress;
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
        if (!CheckHasCharacterSO()) return;

        LoadProgress();
        GetAllCharacterStat();
        ShowCharacter();
        ShowCharacterName();
        ShowCharacterStat();
        ShowProgressSlider(); 
        UpdateLevelUpButtonState();
    }

    private void LoadProgress()
    {
        currentProgress = SaveManager.LoadCharacter(characterSO.ID) ?? characterSO.CreateDefaultProgress();
    }

    private void GetAllCharacterStat()
    {
        if (currentProgress == null)
            currentProgress = characterSO.CreateDefaultProgress();
    }

    private void ShowCharacter()
    {
        if (currentCharacterPreview != null)
        {
            Destroy(currentCharacterPreview);
        }
        if (characterSO?.Avatar != null)
        {
            currentCharacterPreview = Instantiate(characterSO.Avatar, spawnPos.position, spawnPos.rotation);
            currentCharacterPreview.transform.localScale = new Vector3(1.65f, 1.65f, 1f);
        }
    }

    private void ShowCharacterName()
    {
        var level = currentProgress != null ? currentProgress.level : characterSO.BaseLevel;
        characterName.text = characterSO.DisplayName + "(LV." + level +")";
    }

    private void ShowCharacterStat()
    {
        characterHealth.text = (currentProgress != null ? currentProgress.currentHealth : characterSO.healthAmount).ToString();
        characterAtk.text = (currentProgress != null ? currentProgress.attackDamage : characterSO.AttackDamage).ToString();
        characterAtkSpeed.text = characterSO.AttackSpeed.ToString();
    }

    private bool ShowProgressSlider()
    {
        int level = currentProgress != null ? currentProgress.level : characterSO.BaseLevel;
        int shards = currentProgress != null ? currentProgress.shards : characterSO.BaseCharacterShard;

        if (level == 0)
        {
            progressSlider.value = 1f;
        }
        else
        {
            progressSlider.value = (float)shards / (level * 10);
        }

        progressText.text = shards.ToString() + "/" + (level * 10).ToString();
        return progressSlider.value >= 1f;
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
        if (currentProgress == null) LoadProgress();

        int level = currentProgress.level;
        int requireShard = level * 10;
        if (level == 0) requireShard = 0;

        if (currentProgress.shards >= requireShard)
        {
            currentProgress.shards -= requireShard;
            currentProgress.level += 1;

            currentProgress.attackDamage = characterSO.GetAttackDamageForLevel(currentProgress.level);
            currentProgress.currentHealth = characterSO.GetHealthForLevel(currentProgress.level);

            SaveManager.SaveCharacter(currentProgress);
        }
    }
}
