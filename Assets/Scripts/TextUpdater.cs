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
            Text.text = $"{Controller.name}'s Vel:\n{Recoil.Velocity}\nMax Velo:\n{Recoil.MaxVelocity}";
    }
}
