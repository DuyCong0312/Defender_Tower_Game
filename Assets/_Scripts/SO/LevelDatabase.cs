using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Scriptable Object/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public LevelSO[] levels;

    public LevelSO Get(int levelNumber)
    {
        foreach (LevelSO l in levels)
        {
            if (l.levelNumber == levelNumber)
            {
                return l;
            }
        }

        return null;
    }

    public int TotalLevels => levels.Length;
}
