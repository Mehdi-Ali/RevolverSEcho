using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaxRecoil : MonoBehaviour
{
    public XRBaseController controller;
    
    void Start()
    {
        EventSystem.Events.OnRecoilEnd += Placer;
    }

    private void Placer()
    {
        transform.position = controller.transform.position;
    }

    void OnDestroy()
    {
        EventSystem.Events.OnRecoilEnd -= Placer;
    }
}
