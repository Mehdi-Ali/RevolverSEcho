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
    private RecoilPerformance Recoil;

    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += UpdateText;
    }

    void OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= UpdateText;
    }

    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void UpdateText(string _)
    {
        if (Recoil == null)
            Recoil = Controller.GetComponentInChildren<RecoilPerformance>();
        
        if (Recoil != null)
            Text.text = 
            $"MaxVel: {Math.Round(Recoil.MaxVelocity.magnitude, 2)}\nDeltaPos: {Math.Round(Recoil.DeltaPos.magnitude, 2)}\nDeltaRot: {Recoil.DeltaRot}";    

    }

    //delete
    // void Update()
    // {
    //     if (Recoil == null)
    //         Recoil = Controller.GetComponentInChildren<RecoilPerformance>();
        
    //     if (Recoil != null)
    //         Text.text = 
    //         $"Vel: {Recoil.Velocity}";    
    //         //$"MaxVel: {Math.Round(Recoil.MaxVelocity.magnitude, 2)}\nDeltaPos: {Math.Round(Recoil.DeltaPos.magnitude, 2)}\nDeltaRot: {Math.Round(Recoil.DeltaRot.magnitude, 2)}";    
    //     //Debug.Log($"Velocity: {Recoil.Velocity}");   

    // }
}
