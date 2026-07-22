using UnityEngine;

[RequireComponent(typeof(PunchScaleEffect))]
public class TotalKnifeHitsPunchListener : MonoBehaviour
{
    private PunchScaleEffect punchEffect;

    private void Awake()
    {
        punchEffect = GetComponent<PunchScaleEffect>();
    }

    private void OnEnable()
    {
        GameEvents.OnTotalKnifeHitsChanged += HandleTotalKnifeHitsChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnTotalKnifeHitsChanged -= HandleTotalKnifeHitsChanged;
    }

    private void HandleTotalKnifeHitsChanged(int total)
    {
        punchEffect.PlayPunch();
    }
}