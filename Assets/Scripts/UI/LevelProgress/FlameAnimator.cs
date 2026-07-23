using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlameAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameRate = 10f;

    private Image image;
    private float timer;
    private int frameIndex;
    private bool isPlaying;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Play()
    {
        isPlaying = true;
        enabled = true;
    }

    public void Stop()
    {
        isPlaying = false;
        enabled = false;
    }

    private void Update()
    {
        if (!isPlaying || frames == null || frames.Length == 0) return;
        timer += Time.unscaledDeltaTime;
        float frameDuration = 1f / frameRate;
        if (timer < frameDuration) return;
        timer -= frameDuration;
        frameIndex = (frameIndex + 1) % frames.Length;
        image.sprite = frames[frameIndex];
    }
}