using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private ShopPanelController shopPanel;
    [SerializeField] private SettingsPanelController settingsPanel;
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private void Awake()
    {
        playButton.onClick.AddListener(HandlePlayClicked);
        shopButton.onClick.AddListener(HandleShopClicked);
        settingsButton.onClick.AddListener(HandleSettingsClicked);
    }

    private void HandlePlayClicked()
    {
        SceneTransitionManager.Instance.LoadScene(gameplaySceneName);
    }

    private void HandleShopClicked()
    {
        if (shopPanel != null) shopPanel.Show();
    }

    private void HandleSettingsClicked()
    {
        if (settingsPanel != null) settingsPanel.Show();
    }
}