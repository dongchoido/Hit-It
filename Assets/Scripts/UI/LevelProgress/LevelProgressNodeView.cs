using UnityEngine;
using UnityEngine.UI;

public class LevelProgressNodeView : MonoBehaviour
{
    [SerializeField] private Image flameImage;
    [SerializeField] private FlameAnimator flameAnimator;
    [SerializeField] private Sprite unlitFlameSprite;
    [SerializeField] private GameObject bossIcon;
    [SerializeField] private GameObject normalIcon;
    [SerializeField] private float pulseScale = 1.15f;
    [SerializeField] private float pulseSpeed = 2.5f;

    private Vector3 baseScale;
    private bool isPulsing;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Configure(bool isBoss)
    {
        if (bossIcon != null) bossIcon.SetActive(isBoss);
        if (normalIcon != null) normalIcon.SetActive(!isBoss);
    }

    public void SetLocked()
    {
        isPulsing = false;
        transform.localScale = baseScale;
        if (flameAnimator != null) flameAnimator.Stop();
        if (flameImage != null) flameImage.sprite = unlitFlameSprite;
    }

    public void SetCompleted()
    {
        isPulsing = false;
        transform.localScale = baseScale;
        if (flameAnimator != null) flameAnimator.Play();
    }

    public void SetCurrent()
    {
        isPulsing = true;
        if (flameAnimator != null) flameAnimator.Play();
    }

    private void Update()
    {
        if (!isPulsing) return;
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float scaleMultiplier = Mathf.Lerp(1f, pulseScale, t);
        transform.localScale = baseScale * scaleMultiplier;
    }
}