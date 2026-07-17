using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance { get; private set; }

    [SerializeField] private float knifeHitFreeze = 0.02f;
    [SerializeField] private float gameOverFreeze = 0.08f;

    private Coroutine freezeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnKnifeHitLog += HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife += HandleKnifeHitKnife;
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife -= HandleKnifeHitKnife;
    }

    private void HandleKnifeHitLog()
    {
        TriggerFreeze(knifeHitFreeze);
    }

    private void HandleKnifeHitKnife()
    {
        TriggerFreeze(gameOverFreeze);
    }

    private void TriggerFreeze(float duration)
    {
        if (freezeRoutine != null) StopCoroutine(freezeRoutine);
        freezeRoutine = StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}