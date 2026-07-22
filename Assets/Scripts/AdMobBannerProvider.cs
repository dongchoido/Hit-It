using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobBannerProvider : MonoBehaviour, IAdBannerProvider
{
    [SerializeField] private string androidTestAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    private BannerView bannerView;
    private static bool isInitialized;
    private static AdMobBannerProvider instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        if (isInitialized) CreateBannerView();
        else MobileAds.Initialize(HandleInitializationComplete);
    }

    public void ShowBanner()
    {
        if (bannerView == null) CreateBannerView();
        else bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView == null) return;
        bannerView.Hide();
    }

    private void HandleInitializationComplete(InitializationStatus status)
    {
        isInitialized = true;
        CreateBannerView();
    }

    private void CreateBannerView()
    {
        if (bannerView != null) return;

        bannerView = new BannerView(androidTestAdUnitId, AdSize.Banner, AdPosition.Bottom);
        bannerView.OnBannerAdLoaded += HandleBannerLoaded;
        bannerView.OnBannerAdLoadFailed += HandleBannerLoadFailed;

        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    private void HandleBannerLoaded()
    {
        Debug.Log("AdMob banner loaded successfully.");
    }

    private void HandleBannerLoadFailed(LoadAdError error)
    {
        Debug.LogWarning("AdMob banner failed to load: " + error.GetMessage());
    }

    private void OnDestroy()
    {
        if (bannerView == null) return;
        bannerView.Destroy();
    }
}