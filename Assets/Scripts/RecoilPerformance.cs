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


    private Vector3 _velocity;
    private Quaternion _rotation;
    public Vector3 MaxVelocity { get; private set; }
    public Vector3 DeltaPos { get; private set; }
    public float DeltaRot { get; private set; }

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

    private void SetMaxPosRot()
    {
        var controllerTrns = _controller.transform;
        DeltaPos = controllerTrns.position - DeltaPos;
        DeltaRot = Quaternion.Angle(_rotation, controllerTrns.rotation);

        EventSystem.Events.TriggerRecoilEnd(_controller.name);
    }

    private void CalculateVelocity()
    {
        Vector3 currentPosition = _controller.transform.position;
        _velocity = (currentPosition - _previousPosition) / Time.deltaTime;
        MaxVelocity = Vector3.Max(MaxVelocity, _velocity);
        _previousPosition = currentPosition;
    }

    public void StartRecoil(string controllerName)
    {
        if (controllerName != _controller.name)
        return;

        _previousVelocity = Vector3.zero;
        MaxVelocity = Vector3.zero;
        _velocity = Vector3.zero;

        var controllerTrns = _controller.transform;
        DeltaPos = controllerTrns.position;
        _rotation = controllerTrns.rotation;
        InRecoil = true;

        Invoke(nameof(StartCheck), _minimalRecoilSecondes);
    }

    private void StartCheck()
    {
        _previousVelocity = _velocity;
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
        _velocity = Vector3.zero;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }
}
 