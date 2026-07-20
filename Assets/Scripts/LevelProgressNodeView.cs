using UnityEngine;
using UnityEngine.UI;

public class LevelProgressNodeView : MonoBehaviour
{
    [SerializeField] private Image nodeImage;
    [SerializeField] private GameObject bossIcon;
    [SerializeField] private GameObject normalIcon;
    [SerializeField] private Color lockedColor = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] private Color currentColor = Color.white;
    [SerializeField] private Color completedColor = new Color(0.4f, 1f, 0.5f, 1f);
    [SerializeField] private float pulseScale = 1.2f;
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
        if (nodeImage != null) nodeImage.color = lockedColor;
    }

    public void SetCompleted()
    {
        isPulsing = false;
        transform.localScale = baseScale;
        if (nodeImage != null) nodeImage.color = completedColor;
    }

    public void SetCurrent()
    {
        isPulsing = true;
        if (nodeImage != null) nodeImage.color = currentColor;
    }

    private void Update()
    {
        if (!isPulsing) return;
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float scaleMultiplier = Mathf.Lerp(1f, pulseScale, t);
        transform.localScale = baseScale * scaleMultiplier;
    }
}