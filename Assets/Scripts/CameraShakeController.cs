using System.Collections;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    [SerializeField] private float knifeHitDuration = 0.1f;
    [SerializeField] private float knifeHitMagnitude = 0.08f;
    [SerializeField] private float gameOverDuration = 0.25f;
    [SerializeField] private float gameOverMagnitude = 0.25f;
    [SerializeField] private float logBreakDuration = 0.3f;
    [SerializeField] private float logBreakMagnitude = 0.2f;

    private Vector3 originalLocalPosition;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        GameEvents.OnKnifeHitLog += HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife += HandleKnifeHitKnife;
        GameEvents.OnLevelComplete += HandleLevelComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife -= HandleKnifeHitKnife;
        GameEvents.OnLevelComplete -= HandleLevelComplete;
    }

    private void HandleKnifeHitLog()
    {
        StartShake(knifeHitDuration, knifeHitMagnitude);
    }

    private void HandleKnifeHitKnife()
    {
        StartShake(gameOverDuration, gameOverMagnitude);
    }

    private void HandleLevelComplete()
    {
        StartShake(logBreakDuration, logBreakMagnitude);
    }

    private void StartShake(float duration, float magnitude)
    {
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float damper = 1f - elapsed / duration;
            float offsetX = Random.Range(-1f, 1f) * magnitude * damper;
            float offsetY = Random.Range(-1f, 1f) * magnitude * damper;
            transform.localPosition = originalLocalPosition + new Vector3(offsetX, offsetY, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = originalLocalPosition;
    }
}