using UnityEngine;

public class SessionStatsManager : MonoBehaviour
{
    public static SessionStatsManager Instance { get; private set; }

    private int totalKnifeHits;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnKnifeHitLog += HandleKnifeHitLog;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleKnifeHitLog()
    {
        totalKnifeHits++;
        GameEvents.OnTotalKnifeHitsChanged?.Invoke(totalKnifeHits);
    }

    private void HandleGameOver()
    {
        totalKnifeHits = 0;
        GameEvents.OnTotalKnifeHitsChanged?.Invoke(totalKnifeHits);
    }
}