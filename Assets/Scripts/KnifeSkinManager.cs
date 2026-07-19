using UnityEngine;

public class KnifeSkinManager : MonoBehaviour
{
    public static KnifeSkinManager Instance { get; private set; }

    [SerializeField] private KnifeSkinLibrarySO skinLibrary;

    private const string UnlockedPrefix = "SkinUnlocked_";
    private const string EquippedKey = "EquippedSkinId";

    public Color EquippedColor { get; private set; } = Color.white;
    public KnifeSkinLibrarySO Library => skinLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadEquippedColor();
    }

    public bool IsUnlocked(KnifeSkinSO skin)
    {
        if (skin.cost <= 0) return true;
        return PlayerPrefs.GetInt(UnlockedPrefix + skin.skinId, 0) == 1;
    }

    public bool Purchase(KnifeSkinSO skin)
    {
        if (IsUnlocked(skin)) return true;
        if (CurrencyManager.Instance == null) return false;
        bool spent = CurrencyManager.Instance.TrySpend(skin.cost);
        if (!spent) return false;
        PlayerPrefs.SetInt(UnlockedPrefix + skin.skinId, 1);
        PlayerPrefs.Save();
        return true;
    }

    public void Equip(KnifeSkinSO skin)
    {
        if (!IsUnlocked(skin)) return;
        PlayerPrefs.SetString(EquippedKey, skin.skinId);
        PlayerPrefs.Save();
        EquippedColor = skin.tintColor;
        GameEvents.OnKnifeSkinEquipped?.Invoke(EquippedColor);
    }

    public string GetEquippedId()
    {
        return PlayerPrefs.GetString(EquippedKey, string.Empty);
    }

    private void LoadEquippedColor()
    {
        if (skinLibrary == null) return;
        string equippedId = GetEquippedId();
        foreach (KnifeSkinSO skin in skinLibrary.skins)
        {
            if (skin.skinId != equippedId) continue;
            EquippedColor = skin.tintColor;
            return;
        }
    }
}