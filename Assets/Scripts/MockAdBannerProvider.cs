using UnityEngine;

public class MockAdBannerProvider : MonoBehaviour, IAdBannerProvider
{
    [SerializeField] private GameObject bannerVisual;

    public void Initialize()
    {
    }

    public void ShowBanner()
    {
        if (bannerVisual != null) bannerVisual.SetActive(true);
    }

    public void HideBanner()
    {
        if (bannerVisual != null) bannerVisual.SetActive(false);
    }
}