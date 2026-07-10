using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelDataSO currentLevel;
    [SerializeField] private Transform logSpawnPoint;

    private LogController spawnedLog;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        if (currentLevel == null || currentLevel.logPrefab == null) return;

        Vector3 spawnPosition = logSpawnPoint != null ? logSpawnPoint.position : Vector3.zero;
        GameObject logInstance = Instantiate(currentLevel.logPrefab, spawnPosition, Quaternion.identity);
        spawnedLog = logInstance.GetComponent<LogController>();

        if (spawnedLog != null) spawnedLog.Init(currentLevel.rotationPattern);

        GameEvents.OnLevelLoaded?.Invoke(currentLevel.totalKnives);
    }
}