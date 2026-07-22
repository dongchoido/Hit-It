using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class DailyRewardPanelController : MonoBehaviour
{
    [SerializeField] private Button claimButton;
    [SerializeField] private Text rewardAmountText;

    private PanelFader panelFader;

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        claimButton.onClick.AddListener(HandleClaimClicked);
    }

    private void Start()
    {
        if (DailyRewardManager.Instance == null) return;
        if (!DailyRewardManager.Instance.IsRewardAvailable()) return;
        panelFader.Show();
    }

    private void OnEnable()
    {
        GameEvents.OnDailyRewardClaimed += HandleRewardClaimed;
    }

    private void OnDisable()
    {
        GameEvents.OnDailyRewardClaimed -= HandleRewardClaimed;
    }

    private void HandleRewardClaimed(int amount)
    {
        if (rewardAmountText != null) rewardAmountText.text = "+" + amount;
        panelFader.Hide();
    }

    private void HandleClaimClicked()
    {
        if (DailyRewardManager.Instance == null) return;
        DailyRewardManager.Instance.ClaimReward();
    }
}