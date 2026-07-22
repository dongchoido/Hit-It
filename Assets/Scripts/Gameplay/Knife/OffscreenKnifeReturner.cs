using UnityEngine;

public class OffscreenKnifeReturner : MonoBehaviour
{
    [SerializeField] private float returnYThreshold = -15f;

    private void Update()
    {
        if (transform.position.y > returnYThreshold) return;
        PoolManager.Instance.ReturnKnife(gameObject);
    }
}