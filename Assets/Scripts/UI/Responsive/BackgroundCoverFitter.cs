using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundCoverFitter : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private SpriteRenderer spriteRenderer;
    private float lastAspect;
    private float lastOrthoSize;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (targetCamera == null) targetCamera = Camera.main;
        ApplyCoverScale();
    }

    private void Update()
    {
        if (targetCamera == null) return;
        float currentAspect = targetCamera.aspect;
        float currentOrthoSize = targetCamera.orthographicSize;
        if (Mathf.Approximately(currentAspect, lastAspect) && Mathf.Approximately(currentOrthoSize, lastOrthoSize)) return;
        ApplyCoverScale();
    }

    private void ApplyCoverScale()
    {
        lastAspect = targetCamera.aspect;
        lastOrthoSize = targetCamera.orthographicSize;
        float cameraViewHeight = lastOrthoSize * 2f;
        float cameraViewWidth = cameraViewHeight * lastAspect;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float scaleX = cameraViewWidth / spriteWidth;
        float scaleY = cameraViewHeight / spriteHeight;
        float uniformScale = Mathf.Max(scaleX, scaleY);
        transform.localScale = new Vector3(uniformScale, uniformScale, 1f);
    }
}