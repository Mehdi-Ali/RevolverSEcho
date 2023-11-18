using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextUpdater : MonoBehaviour
{
    private TextMeshProUGUI Text;
    public XRBaseController Controller;
    private RecoilPerformance _performance;
    private RecoilEvaluation _evaluation;

    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += UpdateTextNextFrame;
    }

    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
    void UpdateTextNextFrame(string _)
    {
        StartCoroutine(UpdateText());
    }


    private IEnumerator UpdateText()
    {
        yield return null;
        
        if (_performance == null)
            _performance = Controller.GetComponentInChildren<RecoilPerformance>();
        
        if (_evaluation == null)
            _evaluation = Controller.GetComponentInChildren<RecoilEvaluation>();

        if (_performance == null || _evaluation == null) yield break;

        var (_, MaxVelocity, DeltaPos, DeltaRot) = _performance.GetEvaluationStats();

        Text.text = $"VelScore: {Math.Round((float)MaxVelocity, 2)} : {_evaluation.VelocityScore / _evaluation.RecoilType.VelocityFactor}\n" +
                $"PosScore: {Math.Round(DeltaPos, 2)} : {_evaluation.PositionScore / _evaluation.RecoilType.PositionFactor}\n" +
                $"RotScore: {DeltaRot} : {_evaluation.RotationScore / _evaluation.RecoilType.RotationFactor}\n" +
                $"Total Score: {_evaluation.FinalScore}";
    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= UpdateTextNextFrame;
    }
}
