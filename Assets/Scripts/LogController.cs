using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    private RotationPatternSO pattern;
    private Coroutine rotateRoutine;
    private List<RotationPhase> activePhases;

    public void Init(RotationPatternSO rotationPattern)
    {
        pattern = rotationPattern;
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        if (pattern == null || pattern.phases == null || pattern.phases.Length == 0) return;
        BuildActivePhases();
        rotateRoutine = StartCoroutine(RotateRoutine());
    }

    private void BuildActivePhases()
    {
        activePhases = new List<RotationPhase>(pattern.phases);
        if (!pattern.randomizePhaseOrder) return;
        for (int i = activePhases.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            RotationPhase temp = activePhases[i];
            activePhases[i] = activePhases[swapIndex];
            activePhases[swapIndex] = temp;
        }
    }

    private IEnumerator RotateRoutine()
    {
        int index = 0;
        while (true)
        {
            RotationPhase phase = activePhases[index % activePhases.Count];
            float elapsed = 0f;
            while (elapsed < phase.duration)
            {
                float t = phase.duration > 0f ? elapsed / phase.duration : 1f;
                float curveMultiplier = phase.speedCurve.Evaluate(t);
                float currentSpeed = phase.speed * curveMultiplier;
                transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            index++;
            if (pattern.randomizePhaseOrder && index % activePhases.Count == 0) BuildActivePhases();
        }
    }
}