using System.Collections.Generic;
using UnityEngine;

public class KnifeCounterUI : MonoBehaviour
{
    [SerializeField] private GameObject knifeIconPrefab;
    [SerializeField] private Transform iconContainer;

    private readonly List<KnifeIconView> icons = new List<KnifeIconView>();
    private int nextIconToTurnOff;

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += HandleLevelLoaded;
        GameEvents.OnKnifeThrown += HandleKnifeThrown;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnKnifeThrown -= HandleKnifeThrown;
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        foreach (KnifeIconView icon in icons) if (icon != null) Destroy(icon.gameObject);
        icons.Clear();
        nextIconToTurnOff = 0;

        if (knifeIconPrefab == null || iconContainer == null) return;
        for (int i = 0; i < totalKnives; i++)
        {
            GameObject instance = Instantiate(knifeIconPrefab, iconContainer);
            KnifeIconView iconView = instance.GetComponent<KnifeIconView>();
            if (iconView == null) continue;
            iconView.ResetToLit();
            icons.Add(iconView);
        }
    }

    private void HandleKnifeThrown()
    {
        if (nextIconToTurnOff >= icons.Count) return;
        icons[nextIconToTurnOff].PlayTurnOff();
        nextIconToTurnOff++;
    }
}