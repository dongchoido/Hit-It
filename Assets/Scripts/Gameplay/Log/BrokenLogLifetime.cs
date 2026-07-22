using System.Collections;
using UnityEngine;

public class BrokenLogLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float fadeDuration = 0.5f;

    private SpriteRenderer[] pieceRenderers;

    private void Awake()
    {
        pieceRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        float waitBeforeFade = Mathf.Max(0f, lifetime - fadeDuration);
        yield return new WaitForSeconds(waitBeforeFade);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            SetAlpha(Mathf.Lerp(1f, 0f, elapsed / fadeDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        foreach (SpriteRenderer pieceRenderer in pieceRenderers)
        {
            Color color = pieceRenderer.color;
            color.a = alpha;
            pieceRenderer.color = color;
        }
    }
}