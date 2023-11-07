using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class RecoilPerformance : MonoBehaviour
{
    private XRBaseController controller;
    private Vector3 previousPosition;
    public bool InRecoil;
    private bool _canStartCheck;

    [SerializeField] private float _minimalRecoilSecondes = 0.1f;
    [SerializeField] private float _RecoilMinVelocity = 0.1f;

    public Vector3 Velocity {get; private set; }
    public Vector3 MaxVelocity { get; private set; }
    private Vector3 previousVelocity;

    public Vector3 DeltaPos { get; private set; }
    public Vector3 DeltaRot { get; private set; }

    private void Start()
    {
        controller = GetComponentInParent<XRBaseController>();
        previousPosition = controller.transform.position;
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
        bool signCheck = Mathf.Sign(Velocity.y) != Mathf.Sign(previousVelocity.y);
        bool magnitudeCheck = Velocity.magnitude < _RecoilMinVelocity;
    
        if (signCheck || magnitudeCheck)
        {
            var controllerTrns = controller.transform;
            DeltaPos = controllerTrns.position - DeltaPos;
            DeltaRot = controllerTrns.rotation.eulerAngles - DeltaRot;
            EventSystem.Events.TriggerRecoilEnd();
        }

    }

    private void CalculateVelocity()
    {
        Vector3 currentPosition = controller.transform.position;
        Velocity = (currentPosition - previousPosition) / Time.deltaTime;
        MaxVelocity = Vector3.Max(MaxVelocity, Velocity);
        previousPosition = currentPosition;
    }

    public void StartRecoil()
    {
        previousVelocity = Vector3.zero;
        MaxVelocity = Vector3.zero;
        Velocity = Vector3.zero;

        var controllerTrns = controller.transform;
        DeltaPos = controllerTrns.position;
        DeltaRot = controllerTrns.rotation.eulerAngles;
        InRecoil = true;

        Invoke(nameof(StartCheck), _minimalRecoilSecondes);
    }

    private void StartCheck()
    {
        _canStartCheck = true;
    }

    public void EndRecoil()
    {
        InRecoil = false;
        _canStartCheck = false;

        previousVelocity = Vector3.zero;
        MaxVelocity = Vector3.zero;
        Velocity = Vector3.zero;
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }
}
 