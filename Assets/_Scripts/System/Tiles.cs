using UnityEngine;

public class Tiles : MonoBehaviour
{
    public bool hasObject = false;
    public int lockedByID = -1;

    public int col;
    public int row;
    private SpriteRenderer spriteRenderer;
    public bool IsOccupied => hasObject;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Lock(int objectID)
    {
        hasObject = true;
        lockedByID = objectID;
    }

    public void Unlock()
    {
        hasObject = false;
        lockedByID = -1;
    }
    public void SetVisible(bool value)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = value;
    }
}
