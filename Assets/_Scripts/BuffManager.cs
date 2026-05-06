using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject buffDetailsPanel;
    [SerializeField] private GameObject resultBuyPanel;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private TextMeshProUGUI resultBuyText;

    ResourceManager rm;

    private void Start()
    {
        rm = new ResourceManager();
        resultBuyPanel.SetActive(false);
        HideBuffDetailsPanel(); 
        cancelButton.onClick.AddListener(() => CancelButtonOnClick());
        buyButton.onClick.AddListener(() => BuyDiscountBuff());
        UpdateResourceText();
    }

    private void CancelButtonOnClick()
    {
        HideBuffDetailsPanel();
    }

    private void BuyDiscountBuff()
    {
        StartCoroutine(BuyProcess());
    }

    private void HideBuffDetailsPanel()
    {
        buffDetailsPanel.SetActive(false);
    }

    private void UpdateResourceText()
    {
        resourceText.text = rm.GetGold().ToString();
    }

    private IEnumerator BuyProcess()
    {
        if (rm.SpendGold(100))
        {
            resultBuyPanel.SetActive(true);
            resultBuyText.text = "Success";

            UpdateResourceText();
            GameManager.Instance.SaveAndSetDiscount(0.5f);
            HideBuffDetailsPanel();

            yield return new WaitForSeconds(0.5f);
            resultBuyPanel.SetActive(false);
        }
        else
        {
            resultBuyPanel.SetActive(true);
            resultBuyText.text = "Not Enough Gold";
            yield return new WaitForSeconds(0.5f);
            resultBuyPanel.SetActive(false);
            HideBuffDetailsPanel();
        }
    } 
}
