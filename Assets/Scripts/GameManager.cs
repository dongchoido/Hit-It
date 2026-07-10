using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Start, Playing, GameOver, Victory }
    public GameState CurrentState { get; private set; } = GameState.Start;

    private int knivesRemaining;
    private int score;

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
        GameEvents.OnLevelLoaded += HandleLevelLoaded;
        GameEvents.OnKnifeHitLog += HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife += HandleKnifeHitKnife;
        GameEvents.OnLevelComplete += HandleLevelComplete;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife -= HandleKnifeHitKnife;
        GameEvents.OnLevelComplete -= HandleLevelComplete;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        knivesRemaining = totalKnives;
        score = 0;
        CurrentState = GameState.Playing;
        GameEvents.OnGameStart?.Invoke();
        GameEvents.OnSpawnNextKnife?.Invoke();
    }

    private void HandleKnifeHitLog()
    {
        if (CurrentState != GameState.Playing) return;
        score++;
        knivesRemaining--;
        GameEvents.OnScoreChanged?.Invoke(score);
        if (knivesRemaining <= 0) GameEvents.OnLevelComplete?.Invoke();
        else GameEvents.OnSpawnNextKnife?.Invoke();
    }

    private void HandleKnifeHitKnife()
    {
        if (CurrentState != GameState.Playing) return;
        GameEvents.OnGameOver?.Invoke();
    }

    private void HandleLevelComplete()
    {
        CurrentState = GameState.Victory;
    }

    private void HandleGameOver()
    {
        CurrentState = GameState.GameOver;
    }
}