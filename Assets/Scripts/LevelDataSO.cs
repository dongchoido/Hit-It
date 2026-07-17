using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "KnifeHit/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public int totalKnives;
    public GameObject logPrefab;
    public RotationPatternSO rotationPattern;
    public bool isBossLevel;
    public GameObject applePrefab;
    public GameObject obstacleKnifePrefab;
    public float logRadius = 1f;
    public List<ApplePlacement> applePlacements = new List<ApplePlacement>();
    public List<KnifePlacement> obstacleKnifePlacements = new List<KnifePlacement>();
}