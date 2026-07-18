using System.Collections.Generic;
using UnityEngine;

public class LogKnifeSpawner : MonoBehaviour
{
    public void SpawnObstacleKnives(GameObject knifePrefab, List<KnifePlacement> placements, float logRadius)
    {
        if (knifePrefab == null || placements == null) return;
        foreach (KnifePlacement placement in placements)
        {
            GameObject knife = Instantiate(knifePrefab, transform);
            KnifeController controller = knife.GetComponent<KnifeController>();
            float worldSpawnRadius = controller != null ? logRadius + controller.HalfBladeLength : logRadius;
            float localSpawnRadius = ConvertWorldRadiusToLocal(worldSpawnRadius);

            float radians = placement.angleDegrees * Mathf.Deg2Rad;
            Vector3 localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * localSpawnRadius;
            knife.transform.localPosition = localPosition;
            knife.transform.localRotation = Quaternion.Euler(0f, 0f, placement.angleDegrees + 90f);
            CompensateParentScale(knife.transform);
            if (controller != null) controller.SetupAsObstacle();
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