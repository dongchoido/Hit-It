using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class VictoryPanelController : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button homeButton;

    private PanelFader panelFader;

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        playAgainButton.onClick.AddListener(HandlePlayAgainClicked);
        homeButton.onClick.AddListener(HandleHomeClicked);
    }

    private void OnEnable()
    {
        GameEvents.OnClusterVictory += HandleClusterVictory;
        GameEvents.OnLevelLoaded += HandleLevelLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnClusterVictory -= HandleClusterVictory;
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
    }

    private void HandleClusterVictory()
    {
        panelFader.Show();
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        panelFader.Hide();
    }

    private void HandlePlayAgainClicked()
    {
        SceneTransitionManager.Instance.ReloadCurrentScene();
    }

    private void HandleHomeClicked()
    {
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }
}