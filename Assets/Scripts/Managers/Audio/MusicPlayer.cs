using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource activeSource;
    private AudioSource idleSource;
    private Coroutine crossfadeRoutine;
    private float volumeMultiplier = 1f;

    private void Awake()
    {
        activeSource = gameObject.AddComponent<AudioSource>();
        activeSource.loop = true;
        activeSource.playOnAwake = false;
        idleSource = gameObject.AddComponent<AudioSource>();
        idleSource.loop = true;
        idleSource.playOnAwake = false;
    }

    public void SetVolumeMultiplier(float multiplier)
    {
        volumeMultiplier = multiplier;
        if (activeSource.isPlaying) activeSource.volume = volumeMultiplier;
    }

    public void PlayMusic(AudioClip clip, float fadeDuration)
    {
        if (clip == null) return;
        if (activeSource.clip == clip && activeSource.isPlaying) return;
        if (crossfadeRoutine != null) StopCoroutine(crossfadeRoutine);
        crossfadeRoutine = StartCoroutine(CrossfadeRoutine(clip, fadeDuration));
    }

    public void StopMusic(float fadeDuration)
    {
        if (crossfadeRoutine != null) StopCoroutine(crossfadeRoutine);
        crossfadeRoutine = StartCoroutine(FadeOutRoutine(fadeDuration));
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip, float duration)
    {
        idleSource.clip = newClip;
        idleSource.volume = 0f;
        idleSource.Play();
        float elapsed = 0f;
        float startActiveVolume = activeSource.volume;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            activeSource.volume = Mathf.Lerp(startActiveVolume, 0f, t);
            idleSource.volume = Mathf.Lerp(0f, volumeMultiplier, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        activeSource.Stop();
        activeSource.volume = 0f;
        idleSource.volume = volumeMultiplier;
        AudioSource swap = activeSource;
        activeSource = idleSource;
        idleSource = swap;
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float elapsed = 0f;
        float startVolume = activeSource.volume;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            activeSource.volume = Mathf.Lerp(startVolume, 0f, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        activeSource.Stop();
    }
}