using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "KnifeHit/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public int totalKnives;
    public GameObject logPrefab;
    public RotationPatternSO rotationPattern;
    public bool isBossLevel;
}