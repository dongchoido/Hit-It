using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class KnifeController : MonoBehaviour
{
    [SerializeField] private KnifeDataSO knifeData;

    private Rigidbody2D rb;
    private bool hasBeenThrown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetForReuse()
    {
        hasBeenThrown = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.linearVelocity = Vector2.zero;
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
        if (collision.gameObject.CompareTag("Log"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
            transform.SetParent(collision.transform);
            GameEvents.OnLogImpact?.Invoke(collision.GetContact(0).point);
            GameEvents.OnKnifeHitLog?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            GameEvents.OnKnifeHitKnife?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Apple")) return;
        GameEvents.OnAppleCollected?.Invoke();
    }
}