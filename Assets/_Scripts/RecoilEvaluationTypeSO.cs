using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoilEvaluationType", menuName = "ScriptableObjects/Design/RecoilEvaluationType", order = 1)]
public class RecoilEvaluationTypeSO : ScriptableObject
{
    [Header("Ideals")]
    public float IdealPosition;
    public float IdealRotation;
    public float IdealVelocity;

    [Space]
    [Header("Factors")]
    public float PositionFactor;
    public float RotationFactor;
    public float VelocityFactor;

    public Action OnApply;
        
    public void Apply()
    {
        OnApply?.Invoke();
    }
}
