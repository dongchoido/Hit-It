using UnityEngine;

public class KnifeSkinManager : MonoBehaviour
{
    public static KnifeSkinManager Instance { get; private set; }

    [SerializeField] private KnifeSkinLibrarySO skinLibrary;
    [SerializeField] private KnifeSkinSO defaultSkin;

    private const string UnlockedPrefix = "SkinUnlocked_";
    private const string EquippedKey = "EquippedSkinName";

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
        if (skin == defaultSkin) return true;
        return PlayerPrefs.GetInt(UnlockedPrefix + skin.name, 0) == 1;
    }

    public void Unlock(KnifeSkinSO skin)
    {
        PlayerPrefs.SetInt(UnlockedPrefix + skin.name, 1);
        PlayerPrefs.Save();
    }

    public void Equip(KnifeSkinSO skin)
    {
        if (!IsUnlocked(skin)) return;
        PlayerPrefs.SetString(EquippedKey, skin.name);
        PlayerPrefs.Save();
        EquippedSprite = skin.skinSprite;
        GameEvents.OnKnifeSkinEquipped?.Invoke(EquippedSprite);
    }

    public bool IsEquipped(KnifeSkinSO skin)
    {
        return GetEquippedName() == skin.name;
    }

    private string GetEquippedName()
    {
        string defaultName = defaultSkin != null ? defaultSkin.name : string.Empty;
        return PlayerPrefs.GetString(EquippedKey, defaultName);
    }

    private void LoadEquippedSprite()
    {
        if (skinLibrary == null) return;
        string equippedName = GetEquippedName();
        foreach (KnifeSkinSO skin in skinLibrary.skins)
        {
            if (skin.name != equippedName) continue;
            EquippedSprite = skin.skinSprite;
            return;
        }
    }
}