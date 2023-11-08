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
    private XRBaseController _controller;
    private Vector3 _previousPosition;
    private Vector3 _previousVelocity;
    private bool _canStartCheck;


    public Vector3 Velocity {get; private set; }
    public Vector3 MaxVelocity { get; private set; }

    public Vector3 DeltaPos { get; private set; }
    public Vector3 DeltaRot { get; private set; }

    private void Start()
    {
        _controller = GetComponentInParent<XRBaseController>();
        _previousPosition = _controller.transform.position;
        MaxVelocity = Vector3.zero;
        DeltaPos = Vector3.zero;
        DeltaRot = Vector3.zero;
        InRecoil = false;
    }

    void OnEnable()
    {
        EventSystem.Events.OnShoot += StartRecoil;
        EventSystem.Events.OnRecoilEnd += EndRecoil;
    }


    private void Update()
    {
        if (!InRecoil) return;
        CalculateVelocity();

        if (!_canStartCheck) return;
        bool signCheck = Mathf.Sign(Velocity.y) != Mathf.Sign(_previousVelocity.y);
        bool magnitudeCheck = Velocity.magnitude < _RecoilMinVelocity;
    
        if (signCheck || magnitudeCheck)
        {
            //UnityEngine.Debug.Log($"vel: {Velocity.y}, preVel: {_previousVelocity.y}, SignCheck: {signCheck}");
            var controllerTrns = _controller.transform;
            DeltaPos = controllerTrns.position - DeltaPos;
            DeltaRot = controllerTrns.rotation.eulerAngles - DeltaRot;
            EventSystem.Events.TriggerRecoilEnd(_controller.name);
        }
    }

    private void CalculateVelocity()
    {
        Vector3 currentPosition = _controller.transform.position;
        Velocity = (currentPosition - _previousPosition) / Time.deltaTime;
        MaxVelocity = Vector3.Max(MaxVelocity, Velocity);
        _previousPosition = currentPosition;
    }

    public void StartRecoil(string controllerName)
    {
        if (controllerName != _controller.name)
        return;

        _previousVelocity = Vector3.zero;
        MaxVelocity = Vector3.zero;
        Velocity = Vector3.zero;

        var controllerTrns = _controller.transform;
        DeltaPos = controllerTrns.position;
        DeltaRot = controllerTrns.rotation.eulerAngles;
        InRecoil = true;

        Invoke(nameof(StartCheck), _minimalRecoilSecondes);
    }

    private void StartCheck()
    {
        _previousVelocity = Velocity;
        _canStartCheck = true;
    }

    public void EndRecoil(string controllerName)
    {
        if (controllerName != _controller.name)
            return;
        
        InRecoil = false;
        _canStartCheck = false;

        _previousVelocity = Vector3.zero;
        MaxVelocity = Vector3.zero;
        Velocity = Vector3.zero;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }
}
 