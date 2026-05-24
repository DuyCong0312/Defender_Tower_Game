using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level")]
public class LevelSO : ScriptableObject
{
    public int levelNumber;
    public string sceneName;
}