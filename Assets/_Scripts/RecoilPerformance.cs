using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class RecoilPerformance : MonoBehaviour
{
    public bool InRecoil;
    [SerializeField] private float _minimalRecoilSecondes = 0.1f;
    [SerializeField] private float _RecoilMinVelocity = 0.1f;

    private float _finalMaxVelocity;
    private float _finalDeltaPos;
    private float _finalDeltaRot;

    private XRBaseController _controller;
    private Vector3 _previousPosition;
    private Vector3 _previousVelocity;
    private bool _canStartCheck;


    private Vector3 _velocity;
    private Quaternion _rotation;
    private Vector3 _maxVelocity;
    private Vector3 _deltaPos;
    private float _deltaRot;

    private void Start()
    {
        _controller = GetComponentInParent<XRBaseController>();
        _previousPosition = _controller.transform.position;
        InRecoil = false;
    }

    void OnEnable()
    {
        EventSystem.Events.OnShoot += StartRecoil;
        EventSystem.Events.OnRecoilEnd += EndRecoil;
    }


    public void StartRecoil(string controllerName)
    {
        if (controllerName != _controller.name)
        return;

        _previousPosition = Vector3.zero;
        _previousVelocity = Vector3.zero;
        _velocity = Vector3.zero;
        _maxVelocity = Vector3.zero;

        var controllerTrns = _controller.transform;
        _deltaPos = controllerTrns.position;
        _rotation = controllerTrns.rotation;
        InRecoil = true;

        Invoke(nameof(StartCheck), _minimalRecoilSecondes);
    }

    private void StartCheck()
    {
        _previousVelocity = _velocity;
        _canStartCheck = true;
    }

    private void Update()
    {
        if (!InRecoil) return;
        CalculateVelocity();

        if (!_canStartCheck) return;
        bool signCheck = Mathf.Sign(_velocity.y) != Mathf.Sign(_previousVelocity.y);
        bool magnitudeCheck = _velocity.magnitude < _RecoilMinVelocity;
    
        if (signCheck || magnitudeCheck)
        {
            SetMaxPosRot();
        }
    }

    private void CalculateVelocity()
    {
        Vector3 currentPosition = _controller.transform.position;
        _velocity = (currentPosition - _previousPosition) / Time.deltaTime;
        _maxVelocity = Vector3.Max(_maxVelocity, _velocity);
        _previousPosition = currentPosition;
    }

    private void SetMaxPosRot()
    {
        var controllerTrns = _controller.transform;
        _deltaPos = controllerTrns.position - _deltaPos;
        _deltaRot = Quaternion.Angle(_rotation, controllerTrns.rotation);
        UpdateFinalEvaluationStats();
        EventSystem.Events.TriggerRecoilEnd(_controller.name);
    }

    private void UpdateFinalEvaluationStats()
    {
        _finalMaxVelocity = _maxVelocity.magnitude;
        _finalDeltaPos = _deltaPos.magnitude;
        _finalDeltaRot = _deltaRot;
    }


    public void EndRecoil(string controllerName)
    {
        if (controllerName != _controller.name)
            return;
        
        InRecoil = false;
        _canStartCheck = false;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }

    public (float, float, float) GetEvaluationStats()
    {
        return (_finalMaxVelocity, _finalDeltaPos, _finalDeltaRot);
    }
}
 