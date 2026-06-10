using System;
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

    public event Action OnToolDone;
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

        if (hit == null)
        {
            FinishTool();
            return;
        }

        PlacedObject placedObject = hit.GetComponentInParent<PlacedObject>();

        if (placedObject == null || placedObject.Data is not CharacterSO)
        {
            FinishTool();
            return;
        }

        switch (currentTool)
        {
            case ToolType.Move:
                placedObject.MoveToNewPos();
                break;

            case ToolType.Sell:
                placedObject.Sell();
                break;
        }

        FinishTool();
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

    private void FinishTool()
    {
        currentTool = ToolType.None;
        OnToolDone?.Invoke();
    }
}