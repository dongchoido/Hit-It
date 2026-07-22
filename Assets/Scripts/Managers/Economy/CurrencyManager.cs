using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private const string SaveKey = "PlayerApples";

    public int TotalApples { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        TotalApples = PlayerPrefs.GetInt(SaveKey, 0);
    }

    private void OnEnable()
    {
        GameEvents.OnAppleCollected += HandleAppleCollected;
    }

    private void OnDisable()
    {
        GameEvents.OnAppleCollected -= HandleAppleCollected;
    }

    private void HandleAppleCollected(int amount)
    {
        TotalApples += amount;
        PlayerPrefs.SetInt(SaveKey, TotalApples);
        PlayerPrefs.Save();
        GameEvents.OnCurrencyChanged?.Invoke(TotalApples);
    }

    public bool TrySpend(int amount)
    {
        if (amount > TotalApples) return false;
        TotalApples -= amount;
        PlayerPrefs.SetInt(SaveKey, TotalApples);
        PlayerPrefs.Save();
        GameEvents.OnCurrencyChanged?.Invoke(TotalApples);
        return true;
    }
}