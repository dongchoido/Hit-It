using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkinButtonView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text statusText;
    [SerializeField] private Button actionButton;

    private KnifeSkinSO skin;
    private Action<KnifeSkinSO> onClicked;

    private void Awake()
    {
        actionButton.onClick.AddListener(HandleClicked);
    }

    public void Setup(KnifeSkinSO skinData, Action<KnifeSkinSO> clickCallback)
    {
        skin = skinData;
        onClicked = clickCallback;
        if (nameText != null) nameText.text = skin.displayName;
        if (iconImage != null) iconImage.color = skin.tintColor;
        Refresh();
    }

    public void Refresh()
    {
        if (KnifeSkinManager.Instance == null) return;
        bool unlocked = KnifeSkinManager.Instance.IsUnlocked(skin);
        bool equipped = KnifeSkinManager.Instance.GetEquippedId() == skin.skinId;
        if (statusText != null) statusText.text = equipped ? "Đang dùng" : unlocked ? "Trang bị" : skin.cost.ToString();
        actionButton.interactable = !equipped;
    }

    private void HandleClicked()
    {
        onClicked?.Invoke(skin);
    }
}