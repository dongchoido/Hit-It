using UnityEngine;

public class WoodChipSpawner : MonoBehaviour
{
    [SerializeField] private WoodChipVisualLibrarySO chipVisuals;
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private int knifeHitChipCount = 6;
    [SerializeField] private int logBreakChipCount = 24;
    [SerializeField] private float knifeHitForceMin = 1f;
    [SerializeField] private float knifeHitForceMax = 3f;
    [SerializeField] private float logBreakForceMin = 2f;
    [SerializeField] private float logBreakForceMax = 6f;
    [SerializeField] private float gravityScale = 3f;

    private void OnEnable()
    {
        GameEvents.OnLogImpact += HandleLogImpact;
        GameEvents.OnLogBroken += HandleLogBroken;
    }

    private void OnDisable()
    {
        GameEvents.OnLogImpact -= HandleLogImpact;
        GameEvents.OnLogBroken -= HandleLogBroken;
    }

    private void HandleLogImpact(Vector3 position)
    {
        SpawnChips(position, knifeHitChipCount, knifeHitForceMin, knifeHitForceMax);
    }

    private void HandleLogBroken(Vector3 position)
    {
        SpawnChips(position, logBreakChipCount, logBreakForceMin, logBreakForceMax);
    }

    private void SpawnChips(Vector3 position, int count, float forceMin, float forceMax)
    {
        if (chipPrefab == null || chipVisuals == null || chipVisuals.chipSprites == null || chipVisuals.chipSprites.Length == 0) return;
        for (int i = 0; i < count; i++)
        {
            GameObject instance = Instantiate(chipPrefab, position, Quaternion.identity);
            WoodChipController chip = instance.GetComponent<WoodChipController>();
            if (chip == null) continue;
            Sprite randomSprite = chipVisuals.chipSprites[Random.Range(0, chipVisuals.chipSprites.Length)];
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float force = Random.Range(forceMin, forceMax);
            chip.Launch(randomSprite, randomDirection, force, gravityScale);
        }
    }
}