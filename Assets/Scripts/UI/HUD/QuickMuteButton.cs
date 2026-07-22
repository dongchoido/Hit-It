using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuickMuteButton : MonoBehaviour
{
    [SerializeField] private Text label;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClicked);
    }

    private void Start()
    {
        RefreshLabel();
    }

    private void HandleClicked()
    {
        if (AudioSettingsManager.Instance == null) return;
        AudioSettingsManager.Instance.ToggleMute();
        RefreshLabel();
    }

    private void RefreshLabel()
    {
        if (label == null || AudioSettingsManager.Instance == null) return;
        label.text = AudioSettingsManager.Instance.IsMuted ? "Tắt" : "Bật";
    }
}