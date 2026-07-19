using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelDataSO> levelCluster;
    [SerializeField] private Transform logSpawnPoint;
    [SerializeField] private float nextLevelDelay = 1f;

    private LogController spawnedLog;
    private int currentLevelIndex;

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
        GameEvents.OnLevelComplete += HandleLevelComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelComplete -= HandleLevelComplete;
    }

    private void Start()
    {
        currentLevelIndex = 0;
        SpawnLevel(levelCluster[currentLevelIndex]);
    }

    public void RetryCurrentLevel()
    {
        if (spawnedLog != null) Destroy(spawnedLog.gameObject);
        SpawnLevel(levelCluster[currentLevelIndex]);
    }

    private void HandleLevelComplete()
    {
        LevelDataSO completedLevel = levelCluster[currentLevelIndex];
        bool isLastInCluster = completedLevel.isBossLevel || currentLevelIndex >= levelCluster.Count - 1;
        if (isLastInCluster)
        {
            GameEvents.OnClusterVictory?.Invoke();
            return;
        }
        currentLevelIndex++;
        StartCoroutine(LoadNextLevelRoutine());
    }

    private IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        if (spawnedLog != null) Destroy(spawnedLog.gameObject);
        SpawnLevel(levelCluster[currentLevelIndex]);
    }

    private void SpawnLevel(LevelDataSO levelData)
    {
        if (levelData == null || levelData.logPrefab == null) return;
        Vector3 spawnPosition = logSpawnPoint != null ? logSpawnPoint.position : Vector3.zero;
        GameObject logInstance = Instantiate(levelData.logPrefab, spawnPosition, Quaternion.identity);
        spawnedLog = logInstance.GetComponent<LogController>();
        if (spawnedLog != null) spawnedLog.Init(levelData.rotationPattern);
        if (spawnedLog != null) spawnedLog.PlayEntranceAnimation();
        float logSurfaceRadius = spawnedLog != null ? spawnedLog.SurfaceRadius : 0f;
        LogAppleSpawner appleSpawner = logInstance.GetComponent<LogAppleSpawner>();
        if (appleSpawner != null) appleSpawner.SpawnApples(levelData.applePrefab, levelData.applePlacements, logSurfaceRadius, levelData.appleOffsetFromSurface);
        LogKnifeSpawner knifeSpawner = logInstance.GetComponent<LogKnifeSpawner>();
        if (knifeSpawner != null) knifeSpawner.SpawnObstacleKnives(levelData.obstacleKnifePrefab, levelData.obstacleKnifePlacements, logSurfaceRadius);
        GameEvents.OnLevelLoaded?.Invoke(levelData.totalKnives);
        GameEvents.OnLevelMusicChanged?.Invoke(levelData.isBossLevel);
    }
}