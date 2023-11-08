using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(RecoilPerformance))]

public class RecoilEvaluation : MonoBehaviour
{
    [SerializeField] float _idealPosition = 1 / 3;
    [SerializeField] float _idealRotation = 1 / 3;
    [SerializeField] float _idealVelocity = 1 / 3;

    [SerializeField] float _positionFactor = 10f;
    [SerializeField] float _rotationFactor = 0.5f;
    [SerializeField] float _velocityFactor = 45;

    private RecoilPerformance _performance;

    public float PositionScore { get; private set; }
    public float RotationScore { get; private set; }
    public float VelocityScore { get; private set; }
    public float FinalScore { get; private set; }


    void Start()
    {
        _performance = GetComponent<RecoilPerformance>();
    }
    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += CalculateScore;
    }

    public void CalculateScore(string controllerName)
    {
        if (controllerName != transform.parent.name)
            return;

        PositionScore = (float)Math.Round(_positionFactor * Math.Max((_performance.DeltaPos.magnitude / _idealPosition), 1), 2);
        RotationScore = (float)Math.Round(_rotationFactor * Math.Max((_performance.DeltaRot / _idealRotation), 1), 2);
        VelocityScore = (float)Math.Round(_velocityFactor * Math.Max((_performance.MaxVelocity.magnitude / _idealVelocity), 1), 2);

        FinalScore = PositionScore + RotationScore + VelocityScore;

    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= CalculateScore;
    }
}
