using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float scaleFrom = 0.85f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        transform.localScale = Vector3.one * scaleFrom;
    }

    public void Show()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(canvasGroup.alpha, 1f, scaleFrom, 1f));
    }

    public void Hide()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(canvasGroup.alpha, 0f, transform.localScale.x, scaleFrom));
    }

    private IEnumerator FadeRoutine(float fromAlpha, float toAlpha, float fromScale, float toScale)
    {
        canvasGroup.interactable = toAlpha > 0f;
        canvasGroup.blocksRaycasts = toAlpha > 0f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            transform.localScale = Vector3.one * Mathf.Lerp(fromScale, toScale, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = toAlpha;
        transform.localScale = Vector3.one * toScale;
    }
}