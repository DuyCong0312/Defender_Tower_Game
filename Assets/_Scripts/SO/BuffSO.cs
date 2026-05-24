using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Object/Buff")]
public class BuffSO : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private int buffId;
    [SerializeField] private string buffName;
    [SerializeField] private string buffDetail;
    //[SerializeField] private Sprite buffIcon;

    [Header("Gameplay")]
    [SerializeField] private float duration;
    //[SerializeField] private BuffType buffType;
    //[SerializeField] private float value;

    [Header("Shop")]
    [SerializeField] private int price;

    public int BuffID => buffId;
    public string BuffName => buffName;
    public string BuffDetail => buffDetail;
    //public Sprite BuffIcon => buffIcon;

    public float Duration => duration;

    public int Price => price;
}