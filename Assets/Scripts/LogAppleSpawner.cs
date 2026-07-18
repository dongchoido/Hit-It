using System.Collections.Generic;
using UnityEngine;

public class LogAppleSpawner : MonoBehaviour
{
    public void SpawnApples(GameObject applePrefab, List<ApplePlacement> placements, float logRadius, float surfaceOffset)
    {
        if (applePrefab == null || placements == null) return;
        float worldSpawnRadius = logRadius + surfaceOffset;
        float localSpawnRadius = ConvertWorldRadiusToLocal(worldSpawnRadius);
        foreach (ApplePlacement placement in placements)
        {
            float radians = placement.angleDegrees * Mathf.Deg2Rad;
            Vector3 localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * localSpawnRadius;
            GameObject apple = Instantiate(applePrefab, transform);
            apple.transform.localPosition = localPosition;
            apple.transform.localRotation = Quaternion.identity;
            CompensateParentScale(apple.transform);
        }
    }

    private float ConvertWorldRadiusToLocal(float worldRadius)
    {
        float parentScaleX = transform.lossyScale.x;
        return parentScaleX != 0f ? worldRadius / parentScaleX : worldRadius;
    }

    private void CompensateParentScale(Transform target)
    {
        Vector3 parentScale = transform.lossyScale;
        float scaleX = parentScale.x != 0f ? target.localScale.x / parentScale.x : target.localScale.x;
        float scaleY = parentScale.y != 0f ? target.localScale.y / parentScale.y : target.localScale.y;
        float scaleZ = parentScale.z != 0f ? target.localScale.z / parentScale.z : target.localScale.z;
        target.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
}