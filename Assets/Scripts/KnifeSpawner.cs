using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnEnable()
    {
        GameEvents.OnSpawnNextKnife += SpawnKnife;
    }

    private void OnDisable()
    {
        GameEvents.OnSpawnNextKnife -= SpawnKnife;
    }

    private void SpawnKnife()
    {
        GameObject knifeObject = PoolManager.Instance.GetKnife();
        knifeObject.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);
        KnifeController controller = knifeObject.GetComponent<KnifeController>();
        if (controller != null) controller.ResetForReuse();
    }
}