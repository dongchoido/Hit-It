using UnityEngine;

[RequireComponent(typeof(PunchScaleEffect))]
public class CurrencyTextPunchListener : MonoBehaviour
{
    private PunchScaleEffect punchEffect;

    private void Awake()
    {
        punchEffect = GetComponent<PunchScaleEffect>();
    }

    private void OnEnable()
    {
        GameEvents.OnCurrencyChanged += HandleCurrencyChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    private void HandleCurrencyChanged(int total)
    {
        punchEffect.PlayPunch();
    }
}