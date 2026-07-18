using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerAspectAdjuster : MonoBehaviour
{
    [SerializeField] private float portraitMatch = 1f;
    [SerializeField] private float landscapeMatch = 0f;
    [SerializeField] private float landscapeAspectThreshold = 1f;

    private CanvasScaler scaler;
    private float lastAspect;

    private void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        ApplyMatch();
    }

    private void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        if (Mathf.Approximately(currentAspect, lastAspect)) return;
        ApplyMatch();
    }

    private void ApplyMatch()
    {
        lastAspect = (float)Screen.width / Screen.height;
        scaler.matchWidthOrHeight = lastAspect >= landscapeAspectThreshold ? landscapeMatch : portraitMatch;
    }
}