using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private PlaceableSO data;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI priceText;

    void Start()
    {
        if (iconImage != null && data.Avatar!= null)
        {
            var sr = data.Avatar.GetComponent<SpriteRenderer>();
            if (sr != null) iconImage.sprite = sr.sprite;
        }
        UpdateText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GridManager.Instance.ShowAllTiles();
        DragHandler.Instance.StartDrag(data);
    }

    private void UpdateText()
    {
        priceText.text = data.Price.ToString();
    }
}