using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextUpdates : MonoBehaviour
{
    private TextMeshProUGUI Text;
    public XRBaseController Controller;
    private RecoilPerformance _performance;
    private RecoilEvaluation _evolution;

    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += UpdateText;
    }

    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void UpdateText(string _)
    {
        if (_performance == null)
            _performance = Controller.GetComponentInChildren<RecoilPerformance>();
        
        if (_evolution == null)
            _evolution = Controller.GetComponentInChildren<RecoilEvaluation>();

        if (_performance == null || _evolution == null) return;

        Text.text = $"MaxVel: {Math.Round(_performance.MaxVelocity.magnitude, 2)} : {_evolution.VelocityScore}\n" +
                $"DeltaPos: {Math.Round(_performance.DeltaPos.magnitude, 2)} : {_evolution.PositionScore}\n" +
                $"DeltaRot: {_performance.DeltaRot} : {_evolution.RotationScore}";
    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= UpdateText;
    }
}
