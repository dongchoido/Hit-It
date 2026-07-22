using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private bool preventScreenSleep = true;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
        if (preventScreenSleep) Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}