using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private string scoreLabel = "+1";

    private void OnEnable()
    {
        GameEvents.OnLogImpact += HandleLogImpact;
    }

    private void OnDisable()
    {
        GameEvents.OnLogImpact -= HandleLogImpact;
    }

    private void HandleLogImpact(Vector3 position)
    {
        if (floatingTextPrefab == null) return;
        GameObject instance = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        FloatingScoreText floatingText = instance.GetComponent<FloatingScoreText>();
        if (floatingText != null) floatingText.SetText(scoreLabel);
    }
}
