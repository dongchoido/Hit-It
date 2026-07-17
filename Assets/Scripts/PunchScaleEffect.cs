using System.Collections;
using UnityEngine;

public class PunchScaleEffect : MonoBehaviour
{
    [SerializeField] private float punchScale = 1.2f;
    [SerializeField] private float duration = 0.15f;

    private Vector3 originalScale;
    private Coroutine punchRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void PlayPunch()
    {
        if (punchRoutine != null) StopCoroutine(punchRoutine);
        punchRoutine = StartCoroutine(PunchRoutine());
    }

    private IEnumerator PunchRoutine()
    {
        float elapsed = 0f;
        float halfDuration = duration * 0.5f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}