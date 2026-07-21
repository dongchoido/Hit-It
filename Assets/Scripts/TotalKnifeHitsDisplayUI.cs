using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TotalKnifeHitsDisplayUI : MonoBehaviour
{
    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
        label.text = "0";
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
        label.text = total.ToString();
    }
}