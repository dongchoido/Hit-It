using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private IAdBannerProvider bannerProvider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        bannerProvider = GetComponent<IAdBannerProvider>();
        if (bannerProvider != null) bannerProvider.Initialize();
    }

    private void Start()
    {
        ShowBanner();
    }

    public void ShowBanner()
    {
        if (bannerProvider != null) bannerProvider.ShowBanner();
    }

    public void HideBanner()
    {
        if (bannerProvider != null) bannerProvider.HideBanner();
    }
}