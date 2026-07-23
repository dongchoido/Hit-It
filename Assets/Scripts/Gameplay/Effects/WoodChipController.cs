using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class WoodChipController : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float minSpin = -360f;
    [SerializeField] private float maxSpin = 360f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Sprite sprite, Vector2 direction, float force, float gravityScale)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = Color.white;
        rb.gravityScale = gravityScale;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(minSpin, maxSpin);
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        float waitTime = Mathf.Max(0f, lifetime - fadeDuration);
        yield return new WaitForSeconds(waitTime);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}