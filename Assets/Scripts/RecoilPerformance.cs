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
    [SerializeField] private float _RecoilMinVelocity = 0.2f;

    public Vector3 Velocity {get; private set; }
    public Vector3 MaxVelocity { get; private set; }
    private Vector3 previousVelocity;

    public Vector3 Pos {get; private set; }
    public Vector3 MaxPos { get; private set; }

    public Vector3 Rot { get; private set; }
    public Vector3 MaxRot { get; private set; }

    private void Start()
    {
        controller = GetComponentInParent<XRBaseController>();
        previousPosition = controller.transform.position;
        previousVelocity = Vector3.zero;
        MaxVelocity = new();
        MaxPos = new();
        MaxRot = new();
        InRecoil = false;

        EventSystem.Events.OnShoot += StartRecoil;
        EventSystem.Events.OnRecoilEnd += EndRecoil;
    }


    private void Update()
    {
        CalculateVelocity();
        if (!InRecoil) return;
        CalculatingMaxPosRot();

        if(previousVelocity == Vector3.zero)
        {
            previousVelocity = Velocity;
            return;
        }

        if (!_canStartCheck) return;
        bool signCheck = Mathf.Sign(Velocity.y) != Mathf.Sign(previousVelocity.y);
        bool magnitudeCheck = Velocity.magnitude < _RecoilMinVelocity;
    
        if (signCheck || magnitudeCheck)
        {
            EventSystem.Events.TriggerRecoilEnd();
            UnityEngine.Debug.Log("Velocity.magnitude");
        }

    }

    private void CalculateVelocity()
    {
        Vector3 currentPosition = controller.transform.position;
        Velocity = (currentPosition - previousPosition) / Time.deltaTime;
        MaxVelocity = Vector3.Max(MaxVelocity, Velocity);

        previousPosition = currentPosition;
    }

    private void CalculatingMaxPosRot()
    {
        var controllerTrns = controller.transform;
        MaxPos = Vector3.Max(MaxPos, controllerTrns.position);
        MaxRot = Vector3.Max(MaxRot, controllerTrns.rotation.eulerAngles);
    }

    public void StartRecoil()
    {
        previousVelocity = Vector3.zero;
        Velocity = Vector3.zero;
        MaxVelocity = Vector3.zero;

        InRecoil = true;
        Invoke(nameof(StartCheck), _minimalRecoilSecondes);

        var controllerTrns = controller.transform;
        Pos = controllerTrns.position;
        Rot = controllerTrns.rotation.eulerAngles;
    }

    private void StartCheck()
    {
        _canStartCheck = true;
    }

    public void EndRecoil()
    {
        InRecoil = false;
        _canStartCheck = false;

        var controllerTrns = controller.transform;
        Pos = controllerTrns.position - Pos;
        Rot = controllerTrns.rotation.eulerAngles - Rot;
    }

    void OnDestroy()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }
}
 