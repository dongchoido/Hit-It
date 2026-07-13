using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class KnifeController : MonoBehaviour
{
    [SerializeField] private KnifeDataSO knifeData;
    [SerializeField] private float fallGravityScale = 5f;

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

    private void Update()
    {
        if (hasBeenThrown) return;
        if (Input.GetMouseButtonDown(0)) Throw();
    }

    private void Throw()
    {
        hasBeenThrown = true;
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
            FallOff();
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

    private void FallOff()
    {
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = fallGravityScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Apple")) return;
        GameEvents.OnAppleCollected?.Invoke();
    }
}