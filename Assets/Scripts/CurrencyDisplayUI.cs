using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CurrencyDisplayUI : MonoBehaviour
{
    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void Start()
    {
        if (CurrencyManager.Instance != null) label.text = CurrencyManager.Instance.TotalApples.ToString();
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
        label.text = total.ToString();
    }
}