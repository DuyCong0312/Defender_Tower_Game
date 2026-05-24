using UnityEngine;
using UnityEngine.UI;

public class ShowCharacterDetail : MonoBehaviour
{
    [SerializeField] private CharacterSO[] characterSO;
    [SerializeField] private CharacterDetailUI characterDetailUI;

    [SerializeField] private RectTransform buttonMenu;
    [SerializeField] private GameObject buttonPrefab;

    // test
    [SerializeField] private Button testButton;

    private void Start()
    {
        testButton.onClick.AddListener(GiveCharacterShard);
        for (int i = 0; i < characterSO.Length; i++)
        {
            int index = i;
            GameObject characterButton = Instantiate(buttonPrefab, buttonMenu);
            Instantiate(characterSO[i].AvatarUI, characterButton.transform);
            Button button = characterButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    characterDetailUI.SetCharacterSO(characterSO[index]);
                    if (!characterDetailUI.gameObject.activeSelf)
                    {
                        characterDetailUI.gameObject.SetActive(true);
                    }
                    characterDetailUI.ShowAllDetail();
                });
            }
        }
    }

    private void GiveCharacterShard()
    {
        foreach (CharacterSO character in  characterSO)
        {
            character.AddCharacterShard(10, character.DisplayName + "Shard");
        }
        characterDetailUI.ShowAllDetail();
    }
}
