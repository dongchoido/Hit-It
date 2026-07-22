using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class KnifeIconView : MonoBehaviour
{
    [SerializeField] private Color litColor = Color.white;
    [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] private float turnOffDuration = 0.25f;
    [SerializeField] private float punchScale = 1.3f;
    [SerializeField] private float wobbleAngle = 15f;

    private Image iconImage;

    private void Awake()
    {
        iconImage = GetComponent<Image>();
        iconImage.color = litColor;
    }

    public void PlayTurnOff()
    {
        StopAllCoroutines();
        StartCoroutine(TurnOffRoutine());
    }

    public void ResetToLit()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        iconImage.color = litColor;
    }

    private IEnumerator TurnOffRoutine()
    {
        Vector3 originalScale = transform.localScale;
        Quaternion originalRotation = transform.localRotation;
        float punchDuration = turnOffDuration * 0.4f;
        float elapsed = 0f;
        while (elapsed < punchDuration)
        {
            float t = elapsed / punchDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, t);
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, wobbleAngle, t));
            iconImage.color = Color.Lerp(litColor, dimColor, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        float settleDuration = turnOffDuration - punchDuration;
        elapsed = 0f;
        while (elapsed < settleDuration)
        {
            float t = elapsed / settleDuration;
            transform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, t);
            transform.localRotation = Quaternion.Lerp(originalRotation * Quaternion.Euler(0f, 0f, wobbleAngle), originalRotation, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
        iconImage.color = dimColor;
    }
}