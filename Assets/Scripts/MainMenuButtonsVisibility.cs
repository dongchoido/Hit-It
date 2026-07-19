using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MainMenuButtonsVisibility : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.2f;

    private CanvasGroup canvasGroup;
    private int openPanelCount;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        GameEvents.OnModalPanelVisibilityChanged += HandleModalVisibilityChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnModalPanelVisibilityChanged -= HandleModalVisibilityChanged;
    }

    private void HandleModalVisibilityChanged(bool isOpen)
    {
        openPanelCount = Mathf.Max(0, openPanelCount + (isOpen ? 1 : -1));
        float targetAlpha = openPanelCount > 0 ? 0f : 1f;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
        canvasGroup.interactable = targetAlpha > 0.5f;
        canvasGroup.blocksRaycasts = targetAlpha > 0.5f;
    }
}