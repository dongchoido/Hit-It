using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitController : MonoBehaviour
{
    [SerializeField] private float targetWidth = 6f;
    [SerializeField] private float targetHeight = 10.67f;

    private Camera cam;
    private float lastAspect;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyFit();
    }

    private void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        if (Mathf.Approximately(currentAspect, lastAspect)) return;
        ApplyFit();
    }

    private void ApplyFit()
    {
        lastAspect = (float)Screen.width / Screen.height;
        float sizeByHeight = targetHeight * 0.5f;
        float sizeByWidth = targetWidth * 0.5f / lastAspect;
        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }
}