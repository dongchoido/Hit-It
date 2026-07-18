using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private Text totalApplesText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

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
        GameEvents.OnScoreChanged += HandleScoreChanged;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnClusterVictory += HandleClusterVictory;
        GameEvents.OnCurrencyChanged += HandleCurrencyChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnScoreChanged -= HandleScoreChanged;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnClusterVictory -= HandleClusterVictory;
        GameEvents.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    private void Start()
    {
        if (CurrencyManager.Instance != null) HandleCurrencyChanged(CurrencyManager.Instance.TotalApples);
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (scoreText != null) scoreText.text = "0";
    }

    private void HandleScoreChanged(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    private void HandleGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    private void HandleClusterVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    private void HandleCurrencyChanged(int total)
    {
        if (totalApplesText != null) totalApplesText.text = total.ToString();
    }
}