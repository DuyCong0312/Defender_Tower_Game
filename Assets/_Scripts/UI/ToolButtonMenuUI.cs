using UnityEngine;
using UnityEngine.UI;

public class ToolButtonMenuUI : MonoBehaviour
{
    [SerializeField] private Button moveToolButton;
    [SerializeField] private Button sellToolButton;

    private void Start()
    {
        moveToolButton.onClick.AddListener(() => { 
            ToolManager.Instance.SelectTool(ToolType.Move);
        });

        sellToolButton.onClick.AddListener(() => {
            ToolManager.Instance.SelectTool(ToolType.Sell);
        });
    }
}
