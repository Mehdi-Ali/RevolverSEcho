using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartRecoil : MonoBehaviour
{
    public XRBaseController controller;

    void OnEnable()
    {
        EventSystem.Events.OnShoot += Placer;
    }

    private void Placer(string controllerName)
    {
        if (controllerName == controller.name)
            transform.position = controller.transform.position;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= Placer;
    }
}
