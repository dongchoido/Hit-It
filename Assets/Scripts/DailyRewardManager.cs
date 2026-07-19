using System;
using System.Globalization;
using UnityEngine;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance { get; private set; }

    private const string LastClaimKey = "LastDailyRewardClaimUtc";
    [SerializeField] private int rewardAmount = 10;
    [SerializeField] private float cooldownHours = 24f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool IsRewardAvailable()
    {
        string savedValue = PlayerPrefs.GetString(LastClaimKey, string.Empty);
        if (string.IsNullOrEmpty(savedValue)) return true;
        DateTime lastClaim = DateTime.Parse(savedValue, CultureInfo.InvariantCulture);
        return (DateTime.UtcNow - lastClaim).TotalHours >= cooldownHours;
    }

    public void ClaimReward()
    {
        if (!IsRewardAvailable()) return;
        PlayerPrefs.SetString(LastClaimKey, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        PlayerPrefs.Save();
        GameEvents.OnAppleCollected?.Invoke(rewardAmount);
        GameEvents.OnDailyRewardClaimed?.Invoke(rewardAmount);
    }
}