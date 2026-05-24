using UnityEngine;
using UnityEngine.InputSystem;

public enum ToolType
{
    None,
    Move,
    Sell
}

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    public ToolType currentTool = ToolType.None;
    private Camera cam;

    private void Awake() 
    { 
        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (currentTool == ToolType.None) return; 

        var mouse = Mouse.current;
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;
        Vector2 worldPos = cam.ScreenToWorldPoint(mouse.position.ReadValue());

        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit == null) return;

        PlacedObject hitObject = hit.GetComponentInParent<PlacedObject>();
        switch (currentTool)
        {
            case ToolType.Move:
                if(hitObject != null)
                {
                    if (hitObject.Data is not CharacterSO) break;
                    hitObject.MoveToNewPos();
                    ClearTool();
                }
                break;
            case ToolType.Sell:
                if (hitObject != null)
                {
                    if (hitObject.Data is not CharacterSO) break;
                    hitObject.Sell();
                    ClearTool();
                }
                break;
        }
    }

    public void SelectTool(ToolType tool)
    {
        if (currentTool == tool)
        {
            currentTool = ToolType.None;
            return;
        }

        currentTool = tool;
    }

    public void ClearTool() 
    { 
        currentTool = ToolType.None; 
    }
}