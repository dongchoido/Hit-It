using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobBannerProvider : MonoBehaviour, IAdBannerProvider
{
    [SerializeField] private string androidTestAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    private BannerView bannerView;

    public void Initialize()
    {
        MobileAds.Initialize(HandleInitializationComplete);
    }

    public void ShowBanner()
    {
        if (bannerView == null)
        {
            CreateBannerView();
            return;
        }
        bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView == null) return;
        bannerView.Hide();
    }

    private void HandleInitializationComplete(InitializationStatus status)
    {
        CreateBannerView();
    }

    private void CreateBannerView()
    {
        if (bannerView != null) bannerView.Destroy();
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