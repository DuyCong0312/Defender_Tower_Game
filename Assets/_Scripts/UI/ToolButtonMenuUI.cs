using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToolButtonMenuUI : MonoBehaviour
{
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private Button moveToolButton;
    [SerializeField] private Button sellToolButton;
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private TextMeshProUGUI sellText;

    private GameObject dragObject;
    private Vector3 originalPosition;

    private void Start()
    {
        moveToolButton.onClick.AddListener(() => { 
            ToolManager.Instance.SelectTool(ToolType.Move);
            dragObject = moveText.gameObject; 

            if (dragObject == null) return;

            originalPosition = dragObject.transform.position;
        });

        sellToolButton.onClick.AddListener(() => {
            ToolManager.Instance.SelectTool(ToolType.Sell);
            dragObject = sellText.gameObject;

            if (dragObject == null) return;

            originalPosition = dragObject.transform.position;
        });
    }

    private void OnEnable()
    {
        toolManager.OnToolDone += BackToOriginPos;
    }

    private void OnDisable()
    {
        toolManager.OnToolDone -= BackToOriginPos;
    }


    private void Update()
    {
        if (dragObject == null) return;

        dragObject.transform.position = Mouse.current.position.ReadValue();
    }

    private void BackToOriginPos()
    {
        if (dragObject == null) return;

        dragObject.transform.position = originalPosition;
        dragObject = null;
    }
}
