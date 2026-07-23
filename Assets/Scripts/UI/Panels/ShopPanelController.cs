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
    [SerializeField] private GameObject emptyStateLabel;

    private PanelFader panelFader;
    private readonly List<GameObject> spawnedButtons = new List<GameObject>();

    private void Awake()
    {
        panelFader = GetComponent<PanelFader>();
        closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        BuildSkinList();
        panelFader.Show();
    }

    public void Hide()
    {
        panelFader.Hide();
    }

    private void BuildSkinList()
    {
        ClearSkinList();
        if (skinLibrary == null || skinButtonPrefab == null || skinButtonContainer == null) return;
        int ownedCount = 0;
        foreach (KnifeSkinSO skin in skinLibrary.skins)
        {
            if (KnifeSkinManager.Instance == null || !KnifeSkinManager.Instance.IsUnlocked(skin)) continue;
            GameObject instance = Instantiate(skinButtonPrefab, skinButtonContainer);
            ShopSkinButtonView view = instance.GetComponent<ShopSkinButtonView>();
            if (view != null) view.Setup(skin, HandleEquipClicked);
            spawnedButtons.Add(instance);
            ownedCount++;
        }
        if (emptyStateLabel != null) emptyStateLabel.SetActive(ownedCount == 0);
    }

    private void ClearSkinList()
    {
        foreach (GameObject button in spawnedButtons) Destroy(button);
        spawnedButtons.Clear();
    }

    private void HandleEquipClicked(KnifeSkinSO skin)
    {
        if (KnifeSkinManager.Instance == null) return;
        KnifeSkinManager.Instance.Equip(skin);
        BuildSkinList();
    }
}