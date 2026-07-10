using System.Collections;
using UnityEngine;

public class LogController : MonoBehaviour
{
    private RotationPatternSO pattern;
    private Coroutine rotateRoutine;

    public void Init(RotationPatternSO rotationPattern)
    {
        pattern = rotationPattern;
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        if (pattern != null && pattern.speeds.Length > 0) rotateRoutine = StartCoroutine(RotateRoutine());
    }

    private IEnumerator RotateRoutine()
    {
        int index = 0;
        while (true)
        {
            float speed = pattern.speeds[index % pattern.speeds.Length];
            float duration = pattern.durations[index % pattern.durations.Length];
            float elapsed = 0f;
            while (elapsed < duration)
            {
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            index++;
        }
    }
}