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
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
    }

    private void HandleKnifeHitLog()
    {
        totalKnifeHits++;
        GameEvents.OnTotalKnifeHitsChanged?.Invoke(totalKnifeHits);
    }
}