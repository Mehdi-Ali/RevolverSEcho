using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaxRecoil : MonoBehaviour
{
    public XRBaseController controller;
    
    void OnEnable()
    {
        EventSystem.Events.OnRecoilEnd += Placer;
    }

    private void Placer(string controllerName)
    {
        if (controllerName == controller.name)
            transform.position = controller.transform.position;
    }

    void  OnDisable()
    {
        EventSystem.Events.OnRecoilEnd -= Placer;
    }
}
