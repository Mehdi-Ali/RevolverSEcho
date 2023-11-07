using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class RevolverSTrail : MonoBehaviour
{
    private TrailRenderer _trail;

    void Start()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        EventSystem.Events.OnShoot += StartTrail;
        EventSystem.Events.OnRecoilEnd += StopTrail;
    }

    private void StartTrail()
    {
        _trail.emitting = true;
    }

    private void StopTrail()
    {
        _trail.emitting = false;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartTrail;
        EventSystem.Events.OnRecoilEnd -= StopTrail;
    }
}
