using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class TransitionOverlayView : MonoBehaviour
{
    [SerializeField] private RectTransform wipePanel;
    [SerializeField] private float wipeDuration = 0.4f;
    [SerializeField] private AnimationCurve wipeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private CanvasScaler canvasScaler;
    private float offscreenDistance;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        offscreenDistance = ComputeOffscreenDistance();
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

    private float ComputeOffscreenDistance()
    {
        float canvasHalfWidth = canvasScaler.referenceResolution.x * 0.5f;
        float panelHalfWidth = wipePanel.rect.width * 0.5f;
        float panelHalfHeight = wipePanel.rect.height * 0.5f;
        float rotationRadians = wipePanel.eulerAngles.z * Mathf.Deg2Rad;
        float projectedHalfWidth = Mathf.Abs(panelHalfWidth * Mathf.Cos(rotationRadians)) + Mathf.Abs(panelHalfHeight * Mathf.Sin(rotationRadians));
        return canvasHalfWidth + projectedHalfWidth;
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