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
        if (_evolution == null)
            _evolution = Controller.GetComponentInChildren<RecoilEvaluation>();
        
        if (_evolution != null)
            Text.text = 
            $"MaxVel: {_evolution.VelocityScore}\nDeltaPos: {_evolution.PositionScore}\nDeltaRot: {_evolution.RotationScore}";
            //Add the Performance satrats also.
    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= UpdateText;
    }
}
