using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button muteButton;
    [SerializeField] private Text muteButtonLabel;
    [SerializeField] private Button closeButton;

    private PanelFader panelFader;

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        closeButton.onClick.AddListener(Hide);
        musicSlider.onValueChanged.AddListener(HandleMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(HandleSfxSliderChanged);
        muteButton.onClick.AddListener(HandleMuteClicked);
    }

    public void Show()
    {
        RefreshValues();
        panelFader.Show();
    }

    public void Hide()
    {
        panelFader.Hide();
    }

    private void RefreshValues()
    {
        if (AudioSettingsManager.Instance == null) return;
        musicSlider.SetValueWithoutNotify(AudioSettingsManager.Instance.MusicVolume);
        sfxSlider.SetValueWithoutNotify(AudioSettingsManager.Instance.SfxVolume);
        RefreshMuteLabel();
    }

    private void RefreshMuteLabel()
    {
        if (muteButtonLabel == null || AudioSettingsManager.Instance == null) return;
        muteButtonLabel.text = AudioSettingsManager.Instance.IsMuted ? "Bật âm" : "Tắt âm";
    }

    private void HandleMusicSliderChanged(float value)
    {
        if (AudioSettingsManager.Instance != null) AudioSettingsManager.Instance.SetMusicVolume(value);
    }

    private void HandleSfxSliderChanged(float value)
    {
        if (AudioSettingsManager.Instance != null) AudioSettingsManager.Instance.SetSfxVolume(value);
    }

    private void HandleMuteClicked()
    {
        if (AudioSettingsManager.Instance == null) return;
        AudioSettingsManager.Instance.ToggleMute();
        RefreshMuteLabel();
    }
}