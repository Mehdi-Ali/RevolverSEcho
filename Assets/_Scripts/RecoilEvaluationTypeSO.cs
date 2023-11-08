using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoilEvaluationType", menuName = "ScriptableObjects/Design/RecoilEvaluationType", order = 1)]
public class RecoilEvaluationTypeSO : ScriptableObject
{
    [Header("Ideals")]
    public float IdealVelocity;
    public float IdealPosition;
    public float IdealRotation;

    [Space]
    [Header("Factors")]
    public float VelocityFactor;
    public float PositionFactor;
    public float RotationFactor;

    public Action OnApply;
        
    public void Apply()
    {
        OnApply?.Invoke();
    }
}
