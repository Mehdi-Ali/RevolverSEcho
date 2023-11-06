using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TextUpdates : MonoBehaviour
{
    private TextMeshProUGUI Text;
    public XRBaseController Controller;
    private RecoilPerformance Recoil;


    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Recoil == null)
            Recoil = Controller.GetComponentInChildren<RecoilPerformance>();
        
        if (Recoil != null)
            Text.text = 
            $"Vel: {Recoil.Velocity} / {Recoil.MaxVelocity}\nPos: {Recoil.Pos} / {Recoil.MaxPos}\nPos: {Recoil.Rot} / {Recoil.MaxRot}\n{Recoil.InRecoil}\n{Recoil.MaxVelocity.magnitude}";
        //Debug.Log($"Velocity: {Recoil.Velocity}");   
    }
}
