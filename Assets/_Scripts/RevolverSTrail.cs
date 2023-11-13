using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class RevolverSTrail : MonoBehaviour
{
    private TrailRenderer _trail;
    private string _controllerName;

    void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        _controllerName = transform.parent.parent.name;
    }

    void OnEnable()
    {
        EventSystem.Events.OnShoot += StartTrail;
        EventSystem.Events.OnRecoilEnd += StopTrail;
    }

    private void StartTrail(string controllerName)
    {
        if (controllerName == _controllerName)
            _trail.emitting = true;
    }

    private void StopTrail(string controllerName)
    {
        if (controllerName == _controllerName)
            _trail.emitting = false;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartTrail;
        EventSystem.Events.OnRecoilEnd -= StopTrail;
    }
}
