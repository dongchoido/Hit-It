using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip hitLogClip;
    [SerializeField] private AudioClip hitKnifeClip;
    [SerializeField] private AudioClip collectAppleClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnKnifeHitLog += PlayHitLog;
        GameEvents.OnKnifeHitKnife += PlayHitKnife;
        GameEvents.OnAppleCollected += HandleAppleCollected;
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeHitLog -= PlayHitLog;
        GameEvents.OnKnifeHitKnife -= PlayHitKnife;
        GameEvents.OnAppleCollected -= HandleAppleCollected;
    }

    private void HandleAppleCollected(int amount)
    {
        PlayCollectApple();
    }

    private void PlayHitLog()
    {
        if (hitLogClip != null) audioSource.PlayOneShot(hitLogClip);
    }

    private void PlayHitKnife()
    {
        if (hitKnifeClip != null) audioSource.PlayOneShot(hitKnifeClip);
    }

    private void PlayCollectApple()
    {
        if (collectAppleClip != null) audioSource.PlayOneShot(collectAppleClip);
    }
}