using UnityEngine;

public class KnifeSkinManager : MonoBehaviour
{
    public static KnifeSkinManager Instance { get; private set; }

    [SerializeField] private KnifeSkinLibrarySO skinLibrary;

    private const string UnlockedPrefix = "SkinUnlocked_";
    private const string EquippedKey = "EquippedSkinId";

    public Sprite EquippedSprite { get; private set; }
    public KnifeSkinLibrarySO Library => skinLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadEquippedSprite();
    }

    public bool IsUnlocked(KnifeSkinSO skin)
    {
        if (skin.shopCost <= 0) return true;
        return PlayerPrefs.GetInt(UnlockedPrefix + skin.skinId, 0) == 1;
    }

    public void Unlock(KnifeSkinSO skin)
    {
        PlayerPrefs.SetInt(UnlockedPrefix + skin.skinId, 1);
        PlayerPrefs.Save();
    }

    public bool Purchase(KnifeSkinSO skin)
    {
        if (IsUnlocked(skin)) return true;
        if (CurrencyManager.Instance == null) return false;
        bool spent = CurrencyManager.Instance.TrySpend(skin.shopCost);
        if (!spent) return false;
        Unlock(skin);
        return true;
    }

    public void Equip(KnifeSkinSO skin)
    {
        if (!IsUnlocked(skin)) return;
        PlayerPrefs.SetString(EquippedKey, skin.skinId);
        PlayerPrefs.Save();
        EquippedSprite = skin.skinSprite;
        GameEvents.OnKnifeSkinEquipped?.Invoke(EquippedSprite);
    }

    public string GetEquippedId()
    {
        return PlayerPrefs.GetString(EquippedKey, string.Empty);
    }

    private void LoadEquippedSprite()
    {
        if (skinLibrary == null) return;
        string equippedId = GetEquippedId();
        foreach (KnifeSkinSO skin in skinLibrary.skins)
        {
            if (skin.skinId != equippedId) continue;
            EquippedSprite = skin.skinSprite;
            return;
        }
    }
}