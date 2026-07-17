using System.Collections.Generic;
using UnityEngine;

public class LogAppleSpawner : MonoBehaviour
{
    public void SpawnApples(GameObject applePrefab, List<ApplePlacement> placements)
    {
        if (applePrefab == null || placements == null) return;
        foreach (ApplePlacement placement in placements)
        {
            float radians = placement.angleDegrees * Mathf.Deg2Rad;
            Vector3 localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * placement.radius;
            GameObject apple = Instantiate(applePrefab, transform);
            apple.transform.localPosition = localPosition;
            apple.transform.localRotation = Quaternion.identity;
        }
    }
}