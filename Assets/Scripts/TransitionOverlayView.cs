using System.Collections;
using UnityEngine;

public class TransitionOverlayView : MonoBehaviour
{
    [SerializeField] private RectTransform wipePanel;
    [SerializeField] private float wipeDuration = 0.4f;
    [SerializeField] private AnimationCurve wipeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private float offscreenDistance;

    private void Awake()
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
        offscreenDistance = canvasRect.rect.width;
        SetWipeX(-offscreenDistance);
    }

    public IEnumerator PlayCover()
    {
        yield return MoveWipe(-offscreenDistance, 0f);
    }

    public IEnumerator PlayReveal()
    {
        yield return MoveWipe(0f, offscreenDistance);
    }

    private void SetWipeX(float x)
    {
        Vector2 pos = wipePanel.anchoredPosition;
        pos.x = x;
        wipePanel.anchoredPosition = pos;
    }

    private IEnumerator MoveWipe(float fromX, float toX)
    {
        float elapsed = 0f;
        while (elapsed < wipeDuration)
        {
            float t = wipeCurve.Evaluate(elapsed / wipeDuration);
            SetWipeX(Mathf.Lerp(fromX, toX, t));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SetWipeX(toX);
    }
}