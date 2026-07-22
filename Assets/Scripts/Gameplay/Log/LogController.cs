using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField] private GameObject brokenLogPrefab;
    [SerializeField] private float splitForce = 4f;
    [SerializeField] private float brokenPieceLifetime = 2f;
    [SerializeField] private float entranceDuration = 0.45f;
    [SerializeField] private float entranceDropHeight = 5f;
    [SerializeField] private AnimationCurve entranceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private RotationPatternSO pattern;
    private Coroutine rotateRoutine;
    private List<RotationPhase> activePhases;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D logCollider;

    public float SurfaceRadius => logCollider != null ? logCollider.radius * transform.lossyScale.x : 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        logCollider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        GameEvents.OnLevelComplete += HandleLevelComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelComplete -= HandleLevelComplete;
    }

    public void Init(RotationPatternSO rotationPattern)
    {
        pattern = rotationPattern;
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        if (pattern == null || pattern.phases == null || pattern.phases.Length == 0) return;
        BuildActivePhases();
        rotateRoutine = StartCoroutine(RotateRoutine());
    }

    public void PlayEntranceAnimation()
    {
        StartCoroutine(EntranceRoutine());
    }

    public static float GetPrefabSurfaceRadius(GameObject logPrefab)
    {
        if (logPrefab == null) return 0f;
        CircleCollider2D prefabCollider = logPrefab.GetComponent<CircleCollider2D>();
        if (prefabCollider == null) return 0f;
        return prefabCollider.radius * logPrefab.transform.localScale.x;
    }

    private IEnumerator EntranceRoutine()
    {
        Vector3 targetPosition = transform.position;
        Vector3 startPosition = targetPosition + Vector3.up * entranceDropHeight;
        transform.position = startPosition;
        float elapsed = 0f;
        while (elapsed < entranceDuration)
        {
            float t = entranceCurve.Evaluate(elapsed / entranceDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    private void BuildActivePhases()
    {
        activePhases = new List<RotationPhase>(pattern.phases);
        if (!pattern.randomizePhaseOrder) return;
        for (int i = activePhases.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            RotationPhase temp = activePhases[i];
            activePhases[i] = activePhases[swapIndex];
            activePhases[swapIndex] = temp;
        }
    }

    private IEnumerator RotateRoutine()
    {
        int index = 0;
        while (true)
        {
            RotationPhase phase = activePhases[index % activePhases.Count];
            float elapsed = 0f;
            while (elapsed < phase.duration)
            {
                float t = phase.duration > 0f ? elapsed / phase.duration : 1f;
                float curveMultiplier = phase.speedCurve.Evaluate(t);
                float currentSpeed = phase.speed * curveMultiplier;
                transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            index++;
            if (pattern.randomizePhaseOrder && index % activePhases.Count == 0) BuildActivePhases();
        }
    }

    private void HandleLevelComplete()
    {
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        DetachAttachedObjects();
        SpawnBrokenPieces();
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (logCollider != null) logCollider.enabled = false;
    }

    private void DetachAttachedObjects()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            if (child.CompareTag("Knife"))
            {
                KnifeController knife = child.GetComponent<KnifeController>();
                if (knife != null) knife.DetachFromLog(randomDirection);
            }
            else if (child.CompareTag("Apple") && child.gameObject.activeSelf)
            {
                AppleController apple = child.GetComponent<AppleController>();
                if (apple != null) apple.LaunchAway(randomDirection);
            }
        }
    }

    private void SpawnBrokenPieces()
    {
        if (brokenLogPrefab == null) return;
        GameObject broken = Instantiate(brokenLogPrefab, transform.position, transform.rotation);
        Rigidbody2D[] pieceBodies = broken.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D pieceBody in pieceBodies)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            pieceBody.AddForce(randomDirection * splitForce, ForceMode2D.Impulse);
        }
    }
}