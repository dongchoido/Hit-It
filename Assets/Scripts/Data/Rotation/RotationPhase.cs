using System;
using UnityEngine;

[Serializable]
public class RotationPhase
{
    public float speed;
    public float duration;
    public AnimationCurve speedCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
}