using UnityEngine;

[CreateAssetMenu(fileName = "RotationPatternSO", menuName = "KnifeHit/Rotation Pattern")]
public class RotationPatternSO : ScriptableObject
{
    public RotationPhase[] phases;
    public bool randomizePhaseOrder;
}