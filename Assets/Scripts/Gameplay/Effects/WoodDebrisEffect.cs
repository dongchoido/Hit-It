using UnityEngine;

public class WoodDebrisEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;

    private float timer;

    private void OnEnable()
    {
        timer = lifetime;
        ParticleSystem particle = GetComponent<ParticleSystem>();
        if (particle != null) particle.Play();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) PoolManager.Instance.ReturnDebris(gameObject);
    }
}