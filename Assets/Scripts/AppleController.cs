using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class AppleController : MonoBehaviour
{
    [SerializeField] private int appleValue = 1;
    [SerializeField] private GameObject leftHalfPrefab;
    [SerializeField] private GameObject rightHalfPrefab;
    [SerializeField] private float splitForce = 3f;
    [SerializeField] private float halfLifetime = 2f;
    [SerializeField] private float launchForce = 4f;
    [SerializeField] private float launchSpinTorque = 200f;
    [SerializeField] private float launchLifetime = 2f;
    [SerializeField] private float fallGravityScale = 3f;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Knife")) return;
        SpawnHalf(leftHalfPrefab, Vector2.left);
        SpawnHalf(rightHalfPrefab, Vector2.right);
        GameEvents.OnAppleCollected?.Invoke(appleValue);
        gameObject.SetActive(false);
    }

    private void SpawnHalf(GameObject halfPrefab, Vector2 direction)
    {
        if (halfPrefab == null) return;
        GameObject half = Instantiate(halfPrefab, transform.position, transform.rotation);
        Rigidbody2D halfBody = half.GetComponent<Rigidbody2D>();
        if (halfBody != null) halfBody.AddForce(direction * splitForce, ForceMode2D.Impulse);
        Destroy(half, halfLifetime);
    }

    public void LaunchAway(Vector2 direction)
    {
        col.enabled = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.gravityScale = fallGravityScale;
        rb.linearVelocity = direction * launchForce;
        rb.angularVelocity = Random.Range(-launchSpinTorque, launchSpinTorque);
        Destroy(gameObject, launchLifetime);
    }
}