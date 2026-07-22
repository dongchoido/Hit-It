using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] private List<LevelProgressNodeView> nodeViews;
    [SerializeField] private List<Image> connectorFillImages;
    [SerializeField] private float connectorFillDuration = 0.4f;

    private bool isConfigured;

    private void OnEnable()
    {
        GameEvents.OnLevelProgressUpdated += HandleLevelProgressUpdated;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelProgressUpdated -= HandleLevelProgressUpdated;
    }

    private void HandleLevelProgressUpdated(int currentIndex, int totalLevels, bool isBossLevel)
    {
        if (!isConfigured) ConfigureNodes(totalLevels);
        RefreshNodeStates(currentIndex);
    }

    private void ConfigureNodes(int totalLevels)
    {
        for (int i = 0; i < nodeViews.Count; i++)
        {
            bool isBossNode = i == totalLevels - 1;
            nodeViews[i].Configure(isBossNode);
        }
        isConfigured = true;
    }

    private void RefreshNodeStates(int currentIndex)
    {
        for (int i = 0; i < nodeViews.Count; i++)
        {
            if (i < currentIndex) nodeViews[i].SetCompleted();
            else if (i == currentIndex) nodeViews[i].SetCurrent();
            else nodeViews[i].SetLocked();
        }
        for (int i = 0; i < connectorFillImages.Count; i++)
        {
            float targetFill = i < currentIndex ? 1f : 0f;
            StartCoroutine(AnimateConnectorFill(connectorFillImages[i], targetFill));
        }
    }

    private IEnumerator AnimateConnectorFill(Image connector, float targetFill)
    {
        float startFill = connector.fillAmount;
        float elapsed = 0f;
        while (elapsed < connectorFillDuration)
        {
            float t = elapsed / connectorFillDuration;
            connector.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        connector.fillAmount = targetFill;
    }
}