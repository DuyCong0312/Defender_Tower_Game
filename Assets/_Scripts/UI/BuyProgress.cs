using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyProgress : MonoBehaviour
{
    [SerializeField] private GameObject buffDetailsPanel;
    [SerializeField] private GameObject resultBuyPanel;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private TextMeshProUGUI resultBuyText;
    public bool canBuy = false;

    ResourceManager rm;

    // test
    [SerializeField] private Button testButton;

    private void Start()
    {
        rm = new ResourceManager();
        resultBuyPanel.SetActive(false);
        HideBuffDetailsPanel();
        UpdateResourceText();
        testButton.onClick.AddListener(GiveGold);
    }

    public void StartBuyProcess()
    {
        StartCoroutine(BuyProcessItems());
    }

    private void HideBuffDetailsPanel()
    {
        buffDetailsPanel.SetActive(false);
    }

    private void UpdateResourceText()
    {
        resourceText.text = rm.GetGold().ToString();
    }

    private IEnumerator BuyProcessItems()
    {
        if (rm.SpendGold(100))
        {
            canBuy = true;
            resultBuyPanel.SetActive(true);
            resultBuyText.text = "Success";

            UpdateResourceText();
            HideBuffDetailsPanel();

            yield return new WaitForSeconds(0.5f);
            resultBuyPanel.SetActive(false);
        }
        else
        {
            canBuy = false;
            resultBuyPanel.SetActive(true);
            resultBuyText.text = "Not Enough Gold";
            yield return new WaitForSeconds(0.5f);
            resultBuyPanel.SetActive(false);
            HideBuffDetailsPanel();
        }
    }

    // test 
    private void GiveGold() 
    {
        rm.AddGold(100);
        UpdateResourceText();
    }

}

