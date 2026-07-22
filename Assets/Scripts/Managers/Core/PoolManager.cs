using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject woodDebrisPrefab;
    [SerializeField] private int knifePoolSize = 10;
    [SerializeField] private int debrisPoolSize = 10;

    private readonly Queue<GameObject> knifePool = new Queue<GameObject>();
    private readonly Queue<GameObject> debrisPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Prewarm(knifePrefab, knifePool, knifePoolSize);
        Prewarm(woodDebrisPrefab, debrisPool, debrisPoolSize);
    }

    private void Prewarm(GameObject prefab, Queue<GameObject> pool, int count)
    {
        if (prefab == null) return;
        for (int i = 0; i < count; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public GameObject GetKnife()
    {
        return GetFromPool(knifePool, knifePrefab);
    }

    public void ReturnKnife(GameObject knife)
    {
        ReturnToPool(knife, knifePool);
    }

    public GameObject GetDebris()
    {
        return GetFromPool(debrisPool, woodDebrisPrefab);
    }

    public void ReturnDebris(GameObject debris)
    {
        ReturnToPool(debris, debrisPool);
    }

    private GameObject GetFromPool(Queue<GameObject> pool, GameObject prefab)
    {
        GameObject instance = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, transform);
        instance.SetActive(true);
        return instance;
    }

    private void ReturnToPool(GameObject instance, Queue<GameObject> pool)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        pool.Enqueue(instance);
    }
}