using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnGameStart;
    public static Action OnKnifeHitLog;
    public static Action OnKnifeHitKnife;
    public static Action OnAppleCollected;
    public static Action OnLevelComplete;
    public static Action OnClusterVictory;
    public static Action OnGameOver;
    public static Action<int> OnLevelLoaded;
    public static Action<int> OnScoreChanged;
    public static Action OnSpawnNextKnife;
    public static Action<Vector3> OnLogImpact;
}