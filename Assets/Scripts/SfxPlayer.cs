using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private int poolSize = 8;

    private readonly List<AudioSource> sourcePool = new List<AudioSource>();
    private float volumeMultiplier = 1f;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sourcePool.Add(source);
        }
    }

    public void SetVolumeMultiplier(float multiplier)
    {
        volumeMultiplier = multiplier;
    }

    public void PlaySfx(AudioClip clip, float pitch)
    {
        if (clip == null) return;
        AudioSource source = GetAvailableSource();
        source.pitch = pitch;
        source.volume = volumeMultiplier;
        source.clip = clip;
        source.Play();
    }

    public void PlayRandomPitchSfx(AudioClip clip, float pitchMin, float pitchMax)
    {
        PlaySfx(clip, Random.Range(pitchMin, pitchMax));
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in sourcePool) if (!source.isPlaying) return source;
        return sourcePool[0];
    }
}