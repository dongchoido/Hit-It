using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkinButtonView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text statusText;
    [SerializeField] private Button equipButton;

    private KnifeSkinSO skin;
    private Action<KnifeSkinSO> onEquipClicked;

    private void Awake()
    {
        equipButton.onClick.AddListener(HandleEquipClicked);
        if (iconImage != null) iconImage.preserveAspect = true;
    }

    public void Setup(KnifeSkinSO skinData, Action<KnifeSkinSO> equipCallback)
    {
        skin = skinData;
        onEquipClicked = equipCallback;
        if (nameText != null) nameText.text = skin.displayName;
        if (iconImage != null) iconImage.sprite = skin.skinSprite;
        Refresh();
    }

    private void Refresh()
    {
        if (KnifeSkinManager.Instance == null) return;
        bool equipped = KnifeSkinManager.Instance.IsEquipped(skin);
        if (statusText != null) statusText.text = equipped ? "Đang dùng" : string.Empty;
        equipButton.interactable = !equipped;
    }

    private void HandleEquipClicked()
    {
        onEquipClicked?.Invoke(skin);
    }
}