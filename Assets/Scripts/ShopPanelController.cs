using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelFader))]
public class ShopPanelController : MonoBehaviour
{
    [SerializeField] private KnifeSkinLibrarySO skinLibrary;
    [SerializeField] private GameObject skinButtonPrefab;
    [SerializeField] private Transform skinButtonContainer;
    [SerializeField] private Button closeButton;

    private PanelFader panelFader;
    private readonly List<ShopSkinButtonView> spawnedButtons = new List<ShopSkinButtonView>();

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        closeButton.onClick.AddListener(Hide);
        BuildSkinList();
    }

    public void Show()
    {
        RefreshAllButtons();
        panelFader.Show();
    }

    public void Hide()
    {
        panelFader.Hide();
    }

    private void BuildSkinList()
    {
        if (skinLibrary == null || skinButtonPrefab == null || skinButtonContainer == null) return;
        foreach (KnifeSkinSO skin in skinLibrary.skins)
        {
            GameObject instance = Instantiate(skinButtonPrefab, skinButtonContainer);
            ShopSkinButtonView view = instance.GetComponent<ShopSkinButtonView>();
            if (view == null) continue;
            view.Setup(skin, HandleSkinClicked);
            spawnedButtons.Add(view);
        }
    }

    private void HandleSkinClicked(KnifeSkinSO skin)
    {
        if (KnifeSkinManager.Instance == null) return;
        bool unlocked = KnifeSkinManager.Instance.IsUnlocked(skin);
        if (!unlocked) unlocked = KnifeSkinManager.Instance.Purchase(skin);
        if (!unlocked) return;
        KnifeSkinManager.Instance.Equip(skin);
        RefreshAllButtons();
    }

    private void RefreshAllButtons()
    {
        foreach (ShopSkinButtonView view in spawnedButtons) view.Refresh();
    }
}