using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnGameStart;
    public static Action OnKnifeHitLog;
    public static Action OnKnifeHitKnife;
    public static Action<int> OnAppleCollected;
    public static Action OnLevelComplete;
    public static Action OnClusterVictory;
    public static Action OnGameOver;
    public static Action<int> OnLevelLoaded;
    public static Action<int> OnScoreChanged;
    public static Action OnSpawnNextKnife;
    public static Action<Vector3> OnLogImpact;
    public static Action<int> OnCurrencyChanged;
    public static Action OnKnifeThrown;
    public static Action<bool> OnLevelMusicChanged;
    public static Action OnUIButtonClicked;
    public static Action<float> OnMusicVolumeChanged;
    public static Action<float> OnSfxVolumeChanged;
    public static Action<bool> OnAudioMuteChanged;
}