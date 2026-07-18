using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrarySO", menuName = "KnifeHit/Audio Library")]
public class AudioLibrarySO : ScriptableObject
{
    public AudioClip gameplayMusic;
    public AudioClip bossMusic;
    public AudioClip knifeThrowClip;
    public AudioClip knifeHitLogClip;
    public AudioClip knifeHitKnifeClip;
    public AudioClip appleCollectClip;
    public AudioClip logBreakClip;
    public AudioClip gameOverClip;
    public AudioClip victoryClip;
    public AudioClip uiButtonClickClip;
    public float musicFadeDuration = 1f;
    public float sfxPitchVarianceMin = 0.95f;
    public float sfxPitchVarianceMax = 1.05f;
    public float comboPitchStep = 0.03f;
    public float comboPitchMax = 1.5f;
}