using UnityEngine;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string MuteKey = "AudioMuted";

    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public bool IsMuted { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
        SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        IsMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
    }

    private void Start()
    {
        BroadcastCurrentSettings();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.Save();
        GameEvents.OnMusicVolumeChanged?.Invoke(IsMuted ? 0f : MusicVolume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        PlayerPrefs.Save();
        GameEvents.OnSfxVolumeChanged?.Invoke(IsMuted ? 0f : SfxVolume);
    }

    public void ToggleMute()
    {
        IsMuted = !IsMuted;
        PlayerPrefs.SetInt(MuteKey, IsMuted ? 1 : 0);
        PlayerPrefs.Save();
        GameEvents.OnAudioMuteChanged?.Invoke(IsMuted);
        BroadcastCurrentSettings();
    }

    private void BroadcastCurrentSettings()
    {
        GameEvents.OnMusicVolumeChanged?.Invoke(IsMuted ? 0f : MusicVolume);
        GameEvents.OnSfxVolumeChanged?.Invoke(IsMuted ? 0f : SfxVolume);
    }
}