using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(RecoilPerformance))]

public class RecoilEvaluation : MonoBehaviour
{
    [SerializeField] public RecoilEvaluationTypeSO RecoilType; 

    private RecoilPerformance _performance;

    public float PositionScore { get; private set; }
    public float RotationScore { get; private set; }
    public float VelocityScore { get; private set; }
    public float FinalScore { get; private set; }

    private float _positionFactor;
    private float _rotationFactor;
    private float _velocityFactor;
    private float _idealPosition;
    private float _idealRotation;
    private float _idealVelocity;

    private string _controllerName;


    void Start()
    {
        _performance = GetComponent<RecoilPerformance>();
        UpdateRecoilTypeProperties();
        _controllerName = transform.parent.name;
    }

    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += CalculateScore;
        RecoilType.OnApply += UpdateRecoilTypeProperties;
    }

    void UpdateRecoilTypeProperties()
    {
        _positionFactor = RecoilType.PositionFactor;
        _rotationFactor = RecoilType.RotationFactor;
        _velocityFactor = RecoilType.VelocityFactor;
        _idealPosition = RecoilType.IdealPosition;
        _idealRotation = RecoilType.IdealRotation;
        _idealVelocity = RecoilType.IdealVelocity;
    }

    public void CalculateScore(string controllerName)
    {
        if (controllerName != _controllerName)
            return;

        var (id, MaxVelocity, DeltaPos, DeltaRot ) = _performance.GetEvaluationStats();

        VelocityScore = (float)Math.Round(_velocityFactor * Math.Min((MaxVelocity / _idealVelocity), 1), 2);
        PositionScore = (float)Math.Round(_positionFactor * Math.Min((DeltaPos / _idealPosition), 1), 2);
        RotationScore = (float)Math.Round(_rotationFactor * Math.Min((DeltaRot / _idealRotation), 1), 2);

        FinalScore = PositionScore + RotationScore + VelocityScore;

        EventSystem.Events.TriggerOnEvolutionEnd(_controllerName, FinalScore, id);
    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= CalculateScore;
        RecoilType.OnApply -= UpdateRecoilTypeProperties;

    }
}
