using UnityEngine;

public class AppleController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Knife")) return;
        gameObject.SetActive(false);
    }
}