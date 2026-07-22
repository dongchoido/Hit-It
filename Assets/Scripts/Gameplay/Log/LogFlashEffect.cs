using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LogFlashEffect : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.08f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        GameEvents.OnLogImpact += HandleLogImpact;
    }

    private void OnDisable()
    {
        GameEvents.OnLogImpact -= HandleLogImpact;
    }

    private void HandleLogImpact(Vector3 position)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            spriteRenderer.color = Color.Lerp(flashColor, originalColor, elapsed / flashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }
}