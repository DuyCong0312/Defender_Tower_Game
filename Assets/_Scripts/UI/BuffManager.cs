using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject buffDetailsPanel;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private BuyProgress buyProgress;
    [SerializeField] private TextMeshProUGUI buffDetail;


    private BuffSO buffSO;

    public void SetBuffSO(BuffSO buffSO)
    {
        this.buffSO = buffSO;
    }

    private void Start()
    {
        HideBuffDetailsPanel(); 
        cancelButton.onClick.AddListener(() => HideBuffDetailsPanel());
        buyButton.onClick.AddListener(() => {
            buyProgress.StartBuyProcess();
            if (buyProgress.canBuy)
            {
                ApplyTheBuff();
            }
            });
    }

    public void ShowBuffDetailPanel()
    {
        buffDetailsPanel.SetActive(true);
        buffDetail.text = buffSO.BuffDetail;
    }

    public void HideBuffDetailsPanel()
    {
        buffDetailsPanel.SetActive(false);
    }

    private void ApplyTheBuff()
    {
        switch (buffSO.BuffID)
        {
            case 0:
                GameManager.Instance.SaveAndSetDiscount(0.5f);
                break;
            case 1:
                GameManager.Instance.SaveAndSetMultipleShard(10);
                break;
        }
    }
}
