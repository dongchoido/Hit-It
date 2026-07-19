using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class GameOverPanelController : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Text continueCostText;
    [SerializeField] private Text continueTimerText;
    [SerializeField] private int continueCostApples = 5;
    [SerializeField] private float continueTimeLimit = 6f;

    private PanelFader panelFader;
    private Coroutine timerRoutine;

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        retryButton.onClick.AddListener(HandleRetryClicked);
        homeButton.onClick.AddListener(HandleHomeClicked);
        continueButton.onClick.AddListener(HandleContinueClicked);
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnLevelLoaded += HandleLevelLoaded;
        GameEvents.OnCurrencyChanged += HandleCurrencyChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnCurrencyChanged -= HandleCurrencyChanged;
        if (timerRoutine != null) StopCoroutine(timerRoutine);
    }

    private void HandleGameOver()
    {
        panelFader.Show();
        if (continueCostText != null) continueCostText.text = continueCostApples.ToString();
        RefreshContinueAvailability();
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(ContinueTimerRoutine());
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        panelFader.Hide();
        if (timerRoutine != null) StopCoroutine(timerRoutine);
    }

    private void HandleCurrencyChanged(int total)
    {
        RefreshContinueAvailability();
    }

    private void RefreshContinueAvailability()
    {
        if (continueButton == null) return;
        int currentApples = CurrencyManager.Instance != null ? CurrencyManager.Instance.TotalApples : 0;
        continueButton.interactable = currentApples >= continueCostApples;
    }

    private IEnumerator ContinueTimerRoutine()
    {
        float remaining = continueTimeLimit;
        while (remaining > 0f)
        {
            if (continueTimerText != null) continueTimerText.text = Mathf.CeilToInt(remaining).ToString();
            remaining -= Time.unscaledDeltaTime;
            yield return null;
        }
        if (continueButton != null) continueButton.interactable = false;
        if (continueTimerText != null) continueTimerText.text = "0";
    }

    private void HandleRetryClicked()
    {
        SceneTransitionManager.Instance.ReloadCurrentScene();
    }

    private void HandleHomeClicked()
    {
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }

    private void HandleContinueClicked()
    {
        if (CurrencyManager.Instance == null) return;
        bool spent = CurrencyManager.Instance.TrySpend(continueCostApples);
        if (!spent) return;
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        LevelManager.Instance.RetryCurrentLevel();
    }
}