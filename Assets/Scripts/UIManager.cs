using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject knifeIconPrefab;
    [SerializeField] private Transform knifeIconContainer;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private readonly List<GameObject> knifeIcons = new List<GameObject>();

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
        GameEvents.OnKnifeHitLog += HandleKnifeUsed;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnClusterVictory += HandleClusterVictory;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnScoreChanged -= HandleScoreChanged;
        GameEvents.OnKnifeHitLog -= HandleKnifeUsed;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnClusterVictory -= HandleClusterVictory;
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (scoreText != null) scoreText.text = "0";

        foreach (GameObject icon in knifeIcons) Destroy(icon);
        knifeIcons.Clear();

        if (knifeIconPrefab == null || knifeIconContainer == null) return;
        for (int i = 0; i < totalKnives; i++)
        {
            GameObject icon = Instantiate(knifeIconPrefab, knifeIconContainer);
            knifeIcons.Add(icon);
        }
        Debug.Log(totalKnives);
    }

    private void HandleScoreChanged(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    private void HandleKnifeUsed()
    {
        if (knifeIcons.Count == 0) return;
        int lastIndex = knifeIcons.Count - 1;
        Destroy(knifeIcons[lastIndex]);
        knifeIcons.RemoveAt(lastIndex);
    }

    private void HandleGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    private void HandleClusterVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }
}