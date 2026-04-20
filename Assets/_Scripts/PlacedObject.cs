using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public PlaceableSO Data { get; private set; }
    public Tiles OriginTile { get; private set; }
    public List<Tiles> OwnedTiles { get; private set; }

    public void Initialize(PlaceableSO so, List<Tiles> ownedTiles)
    {
        Data = so;
        OwnedTiles = ownedTiles;
    }

    public void RemoveFromGrid()
    {
        GridManager.Instance.UnlockArea(OwnedTiles);
    }
}