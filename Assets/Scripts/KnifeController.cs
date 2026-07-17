using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class KnifeController : MonoBehaviour
{
    [SerializeField] private KnifeDataSO knifeData;
    [SerializeField] private float fallGravityScale = 5f;
    [SerializeField] private float knockbackForce = 4f;
    [SerializeField] private float maxSpinTorque = 250f;
    [SerializeField] private float anticipationDuration = 0.08f;
    [SerializeField] private float anticipationOffset = 0.15f;
    [SerializeField] private AnimationCurve anticipationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Rigidbody2D rb;
    private Collider2D col;
    private bool hasBeenThrown;
    private bool hasResolvedCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void ResetForReuse()
    {
        StopAllCoroutines();
        hasBeenThrown = false;
        hasResolvedCollision = false;
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        rb.isKinematic = false;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        col.enabled = true;
    }

    public void SetupAsObstacle()
    {
        hasBeenThrown = true;
        hasResolvedCollision = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        col.enabled = true;
    }

    private void Update()
    {
        if (hasBeenThrown) return;
        if (Input.GetMouseButtonDown(0)) StartCoroutine(AnticipateAndThrow());
    }

    private IEnumerator AnticipateAndThrow()
    {
        hasBeenThrown = true;
        Vector3 startPosition = transform.position;
        Vector3 pulledPosition = startPosition + Vector3.down * anticipationOffset;
        float elapsed = 0f;
        while (elapsed < anticipationDuration)
        {
            float t = anticipationCurve.Evaluate(elapsed / anticipationDuration);
            transform.position = Vector3.Lerp(startPosition, pulledPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        rb.linearVelocity = Vector2.up * knifeData.throwForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasResolvedCollision) return;

        if (collision.gameObject.CompareTag("Log"))
        {
            hasResolvedCollision = true;
            StickToLog(collision);
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            hasResolvedCollision = true;
            Vector2 knockDirection = (transform.position - collision.transform.position).normalized;
            FallOff(knockDirection);
            GameEvents.OnKnifeHitKnife?.Invoke();
        }
    }

    private void StickToLog(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        transform.SetParent(collision.transform);
        GameEvents.OnLogImpact?.Invoke(collision.GetContact(0).point);
        GameEvents.OnKnifeHitLog?.Invoke();
    }

    private void FallOff(Vector2 knockDirection)
    {
        col.enabled = false;
        rb.linearVelocity = knockDirection * knockbackForce;
        rb.angularVelocity = Random.Range(-maxSpinTorque, maxSpinTorque);
        rb.gravityScale = fallGravityScale;
    }

    public void DetachFromLog(Vector2 knockDirection)
    {
        hasResolvedCollision = true;
        col.enabled = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.linearVelocity = knockDirection * knockbackForce;
        rb.angularVelocity = Random.Range(-maxSpinTorque, maxSpinTorque);
        rb.gravityScale = fallGravityScale;
    }
}