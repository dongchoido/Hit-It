using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class GachaPanelController : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Text resultLabel;
    [SerializeField] private Text costText;
    [SerializeField] private GachaConfigSO config;
    [SerializeField] private float spinAnimDuration = 1.2f;
    [SerializeField] private float spinAnimInterval = 0.08f;

    private PanelFader panelFader;

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        spinButton.onClick.AddListener(HandleSpinClicked);
        closeButton.onClick.AddListener(Hide);
        if (resultIcon != null) resultIcon.preserveAspect = true;
    }

    public void Show()
    {
        RefreshCostText();
        if (resultLabel != null) resultLabel.text = string.Empty;
        panelFader.Show();
    }

    public void Hide()
    {
        panelFader.Hide();
    }

    private void RefreshCostText()
    {
        if (costText != null && GachaManager.Instance != null) costText.text = GachaManager.Instance.SpinCost.ToString();
    }

    private void HandleSpinClicked()
    {
        if (GachaManager.Instance == null) return;
        spinButton.interactable = false;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        float elapsed = 0f;
        while (elapsed < spinAnimDuration)
        {
            ShowRandomPreviewIcon();
            yield return new WaitForSecondsRealtime(spinAnimInterval);
            elapsed += spinAnimInterval;
        }
        GachaSpinResult result = GachaManager.Instance.Spin();
        ShowResult(result);
        spinButton.interactable = true;
    }

    private void ShowRandomPreviewIcon()
    {
        if (config == null || config.entries.Count == 0 || resultIcon == null) return;
        GachaEntry entry = config.entries[Random.Range(0, config.entries.Count)];
        resultIcon.sprite = entry.skin != null ? entry.skin.skinSprite : null;
    }

    private void ShowResult(GachaSpinResult result)
    {
        if (!result.Success)
        {
            if (resultLabel != null) resultLabel.text = "Không đủ táo";
            return;
        }
        if (result.WonSkin == null)
        {
            if (resultLabel != null) resultLabel.text = "Chúc bạn may mắn lần sau";
            if (resultIcon != null) resultIcon.sprite = null;
            return;
        }
        if (resultIcon != null) resultIcon.sprite = result.WonSkin.skinSprite;
        if (resultLabel != null) resultLabel.text = "Nhận được: " + result.WonSkin.displayName;
    }
}