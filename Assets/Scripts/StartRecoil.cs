using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartRecoil : MonoBehaviour
{
    public XRBaseController controller;

    void Start()
    {
        EventSystem.Events.OnShoot += Placer;
    }

    private void Placer()
    {
        transform.position = controller.transform.position;
    }

    void OnDestroy()
    {
        EventSystem.Events.OnShoot -= Placer;
    }
}
