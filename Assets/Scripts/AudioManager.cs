using UnityEngine;

[RequireComponent(typeof(MusicPlayer))]
[RequireComponent(typeof(SfxPlayer))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioLibrarySO audioLibrary;

    private MusicPlayer musicPlayer;
    private SfxPlayer sfxPlayer;
    private int knifeComboCount;
    private int appleComboCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        musicPlayer = GetComponent<MusicPlayer>();
        sfxPlayer = GetComponent<SfxPlayer>();
    }

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += HandleLevelLoaded;
        GameEvents.OnLevelMusicChanged += HandleLevelMusicChanged;
        GameEvents.OnKnifeThrown += HandleKnifeThrown;
        GameEvents.OnKnifeHitLog += HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife += HandleKnifeHitKnife;
        GameEvents.OnAppleCollected += HandleAppleCollected;
        GameEvents.OnLevelComplete += HandleLevelComplete;
        GameEvents.OnClusterVictory += HandleClusterVictory;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnUIButtonClicked += HandleUIButtonClicked;
        GameEvents.OnMusicVolumeChanged += HandleMusicVolumeChanged;
        GameEvents.OnSfxVolumeChanged += HandleSfxVolumeChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelLoaded -= HandleLevelLoaded;
        GameEvents.OnLevelMusicChanged -= HandleLevelMusicChanged;
        GameEvents.OnKnifeThrown -= HandleKnifeThrown;
        GameEvents.OnKnifeHitLog -= HandleKnifeHitLog;
        GameEvents.OnKnifeHitKnife -= HandleKnifeHitKnife;
        GameEvents.OnAppleCollected -= HandleAppleCollected;
        GameEvents.OnLevelComplete -= HandleLevelComplete;
        GameEvents.OnClusterVictory -= HandleClusterVictory;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnUIButtonClicked -= HandleUIButtonClicked;
        GameEvents.OnMusicVolumeChanged -= HandleMusicVolumeChanged;
        GameEvents.OnSfxVolumeChanged -= HandleSfxVolumeChanged;
    }

    private void HandleLevelLoaded(int totalKnives)
    {
        knifeComboCount = 0;
        appleComboCount = 0;
    }

    private void HandleLevelMusicChanged(bool isBossLevel)
    {
        AudioClip targetClip = isBossLevel ? audioLibrary.bossMusic : audioLibrary.gameplayMusic;
        musicPlayer.PlayMusic(targetClip, audioLibrary.musicFadeDuration);
    }

    private void HandleKnifeThrown()
    {
        sfxPlayer.PlayRandomPitchSfx(audioLibrary.knifeThrowClip, audioLibrary.sfxPitchVarianceMin, audioLibrary.sfxPitchVarianceMax);
    }

    private void HandleKnifeHitLog()
    {
        float basePitch = Mathf.Min(1f + knifeComboCount * audioLibrary.comboPitchStep, audioLibrary.comboPitchMax);
        float jitteredPitch = basePitch + Random.Range(-audioLibrary.comboPitchJitter, audioLibrary.comboPitchJitter);
        sfxPlayer.PlaySfx(audioLibrary.knifeHitLogClip, jitteredPitch);
        knifeComboCount++;
        if (audioLibrary.comboMilestoneStep > 0 && knifeComboCount % audioLibrary.comboMilestoneStep == 0) sfxPlayer.PlaySfx(audioLibrary.comboMilestoneClip, 1f);
    }

    private void HandleKnifeHitKnife()
    {
        sfxPlayer.PlaySfx(audioLibrary.knifeHitKnifeClip, 1f);
        musicPlayer.StopMusic(audioLibrary.musicFadeDuration);
    }

    private void HandleAppleCollected(int amount)
    {
        float pitch = Mathf.Min(1f + appleComboCount * audioLibrary.comboPitchStep, audioLibrary.comboPitchMax);
        sfxPlayer.PlaySfx(audioLibrary.appleCollectClip, pitch);
        appleComboCount++;
    }

    private void HandleLevelComplete()
    {
        sfxPlayer.PlaySfx(audioLibrary.logBreakClip, 1f);
    }

    private void HandleClusterVictory()
    {
        sfxPlayer.PlaySfx(audioLibrary.victoryClip, 1f);
        musicPlayer.StopMusic(audioLibrary.musicFadeDuration);
    }

    private void HandleGameOver()
    {
        sfxPlayer.PlaySfx(audioLibrary.gameOverClip, 1f);
        musicPlayer.StopMusic(audioLibrary.musicFadeDuration);
    }

    private void HandleUIButtonClicked()
    {
        sfxPlayer.PlaySfx(audioLibrary.uiButtonClickClip, 1f);
    }

    private void HandleMusicVolumeChanged(float volume)
    {
        musicPlayer.SetVolumeMultiplier(volume);
    }

    private void HandleSfxVolumeChanged(float volume)
    {
        sfxPlayer.SetVolumeMultiplier(volume);
    }
}