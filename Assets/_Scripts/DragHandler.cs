using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragHandler : MonoBehaviour
{
    public static DragHandler Instance { get; private set; }

    [Header("Colors")]
    [SerializeField] private Color validColor = new Color(0f, 1f, 0f, 0.4f);
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.4f);

    private PlaceableSO currentSO;
    private GameObject ghostObject;
    private List<Tiles> highlightedTiles = new();
    private List<SpriteRenderer> tileRenderers = new();
    private Camera mainCam;

    public bool IsDragging => currentSO != null;

    void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
    }

    public void StartDrag(PlaceableSO so)
    {
        if (IsDragging) CancelDrag();

        currentSO = so;
        ghostObject = Instantiate(so.Avatar != null ? so.Avatar : so.Prefab);
        SetGhostAlpha(ghostObject, 0.2f);
        ghostObject.name = "Ghost_" + so.DisplayName;
    }

    void Update()
    {
        if (!IsDragging) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 screenPos = mouse.position.ReadValue();
        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(screenPos);

        ghostObject.transform.position = mouseWorld;

        var centerTile = GridManager.Instance.GetTileAtWorldPos(mouseWorld);

        Tiles originTile = null;
        if (centerTile != null)
        {
            originTile = GridManager.Instance.GetOriginFromCenter(centerTile, currentSO.GridSize);
        }
        UpdateHighlight(originTile);

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            if (originTile != null && 
                GridManager.Instance.CanPlace(originTile, currentSO.GridSize) && 
                GameManager.Instance.coinAmount >= (currentSO.Price * (1 - GameManager.Instance.discount)))
                ConfirmDrop(originTile);
            else
                CancelDrag();
        }

        if (mouse.rightButton.wasPressedThisFrame ||
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CancelDrag();
        }
    }

    void UpdateHighlight(Tiles originTile)
    {
        ClearHighlight();
        if (originTile == null) return;

        bool canPlace = GridManager.Instance.CanPlace(originTile, currentSO.GridSize);
        Color color = canPlace ? validColor : invalidColor;

        highlightedTiles = GridManager.Instance.GetTilesInArea(originTile, currentSO.GridSize);

        foreach (var tile in highlightedTiles)
        {
            var sr = tile.GetComponent<SpriteRenderer>();
            if (sr == null) continue;
            tileRenderers.Add(sr);
            sr.color = color;
        }
    }

    void ClearHighlight()
    {
        foreach (var sr in tileRenderers)
            if (sr != null)
            {
                sr.color = new Color(1f, 1f, 1f, 0.2f);
            }

        tileRenderers.Clear();
        highlightedTiles.Clear();
    }

    void ConfirmDrop(Tiles originTile)
    {
        Vector3 centerPos = CalculateCenterPosition(originTile, currentSO.GridSize);

        var placed = Instantiate(currentSO.Prefab, centerPos, Quaternion.identity);

        var placedObj = placed.GetComponent<PlacedObject>();
        placedObj.Initialize(
            currentSO,
            GridManager.Instance.LockArea(originTile, currentSO.GridSize, currentSO.ID)
        );

        GridManager.Instance.HideAllTiles();
        GameManager.Instance.DecreaseCoinAmount(currentSO.Price);
        CleanupDrag();
    }

    public void CancelDrag()
    {
        CleanupDrag();
    }

    void CleanupDrag()
    {
        ClearHighlight();
        if (ghostObject != null) Destroy(ghostObject);
        GridManager.Instance.HideAllTiles();
        ghostObject = null;
        currentSO = null;
    }

    Vector3 CalculateCenterPosition(Tiles origin, Vector2Int size)
    {
        var tilesInArea = GridManager.Instance.GetTilesInArea(origin, size);
        if (tilesInArea.Count == 0) return origin.transform.position;

        Vector3 sum = Vector3.zero;
        foreach (var t in tilesInArea)
            sum += t.transform.position;

        return sum / tilesInArea.Count;
    }

    void SetGhostAlpha(GameObject obj, float alpha)
    {
        foreach (var sr in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            var c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}