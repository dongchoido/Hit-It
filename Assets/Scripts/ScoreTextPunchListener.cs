using UnityEngine;

[RequireComponent(typeof(PunchScaleEffect))]
public class ScoreTextPunchListener : MonoBehaviour
{
    private PunchScaleEffect punchEffect;

    private void Awake()
    {
        punchEffect = GetComponent<PunchScaleEffect>();
    }

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += HandleScoreChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(int score)
    {
        punchEffect.PlayPunch();
    }
}