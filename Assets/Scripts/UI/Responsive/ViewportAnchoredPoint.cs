using UnityEngine;

public class ViewportAnchoredPoint : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Vector2 viewportPosition = new Vector2(0.5f, 0.1f);
    [SerializeField] private float depth = 10f;

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        ApplyPosition();
    }

    private void Update()
    {
        ApplyPosition();
    }

    private void ApplyPosition()
    {
        if (targetCamera == null) return;
        Vector3 viewportPoint = new Vector3(viewportPosition.x, viewportPosition.y, depth);
        Vector3 worldPoint = targetCamera.ViewportToWorldPoint(viewportPoint);
        worldPoint.z = transform.position.z;
        transform.position = worldPoint;
    }
}