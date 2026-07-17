using System.Collections.Generic;
using UnityEngine;

public class LogKnifeSpawner : MonoBehaviour
{
    public void SpawnObstacleKnives(GameObject knifePrefab, List<KnifePlacement> placements)
    {
        if (knifePrefab == null || placements == null) return;
        foreach (KnifePlacement placement in placements)
        {
            float radians = placement.angleDegrees * Mathf.Deg2Rad;
            Vector3 localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * placement.radius;
            GameObject knife = Instantiate(knifePrefab, transform);
            knife.transform.localPosition = localPosition;
            knife.transform.localRotation = Quaternion.Euler(0f, 0f, placement.angleDegrees + 90f);
            KnifeController controller = knife.GetComponent<KnifeController>();
            if (controller != null) controller.SetupAsObstacle();
        }
    }
}