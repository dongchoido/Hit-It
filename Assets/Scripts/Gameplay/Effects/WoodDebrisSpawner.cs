using UnityEngine;

public class WoodDebrisSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnLogImpact += SpawnDebris;
    }

    private void OnDisable()
    {
        GameEvents.OnLogImpact -= SpawnDebris;
    }

    private void SpawnDebris(Vector3 position)
    {
        GameObject debris = PoolManager.Instance.GetDebris();
        debris.transform.position = position;
        debris.transform.rotation = Quaternion.identity;
    }
}