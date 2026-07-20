using UnityEngine;

public class AppleGlowPulse : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glowRenderer;
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.55f;
    [SerializeField] private float pulseSpeed = 1.2f;

    private float randomOffset;
    private bool isPulsing;

    private void Awake()
    {
        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    public void StartPulsing()
    {
        isPulsing = true;
        enabled = true;
    }

    public void StopPulsing()
    {
        isPulsing = false;
        enabled = false;
        if (glowRenderer == null) return;
        Color color = glowRenderer.color;
        color.a = 0f;
        glowRenderer.color = color;
    }

    private void Update()
    {
        if (!isPulsing || glowRenderer == null) return;
        float t = (Mathf.Sin(Time.time * pulseSpeed + randomOffset) + 1f) * 0.5f;
        Color color = glowRenderer.color;
        color.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        glowRenderer.color = color;
    }
}