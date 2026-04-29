using UnityEngine;

public abstract class PlaceableSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string displayName;
    [SerializeField] private float price;
    [SerializeField] private GameObject avatar;
    [SerializeField] private GameObject prefab;

    [Header("Grid Occupation")]
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;

    [Header("Stat")]
    [SerializeField] private float healthAmount = 10f;

    public int ID => id;
    public string DisplayName => displayName;
    public float Price => price;
    public GameObject Avatar => avatar;
    public GameObject Prefab => prefab;
    public Vector2Int GridSize => gridSize;
    public float HealthAmount => healthAmount;
}