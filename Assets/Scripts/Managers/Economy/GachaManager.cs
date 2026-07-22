using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance { get; private set; }

    [SerializeField] private GachaConfigSO config;
    [SerializeField] private int spinCost = 20;

    public int SpinCost => spinCost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GachaSpinResult Spin()
    {
        if (CurrencyManager.Instance == null) return GachaSpinResult.Failed();
        bool spent = CurrencyManager.Instance.TrySpend(spinCost);
        if (!spent) return GachaSpinResult.Failed();
        KnifeSkinSO wonSkin = PickWeightedEntry();
        if (wonSkin != null && KnifeSkinManager.Instance != null) KnifeSkinManager.Instance.Unlock(wonSkin);
        GachaSpinResult result = new GachaSpinResult(true, wonSkin);
        GameEvents.OnGachaSpinCompleted?.Invoke(result);
        return result;
    }

    private KnifeSkinSO PickWeightedEntry()
    {
        if (config == null || config.entries.Count == 0) return null;
        float totalWeight = 0f;
        foreach (GachaEntry entry in config.entries) totalWeight += entry.weight;
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        foreach (GachaEntry entry in config.entries)
        {
            cumulative += entry.weight;
            if (roll <= cumulative) return entry.skin;
        }
        return null;
    }
}