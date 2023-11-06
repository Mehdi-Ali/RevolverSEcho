using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecoilPerformance : MonoBehaviour
{
    private XRBaseController controller;
    private Vector2 previousPosition;
    public bool InRecoil;
    private bool _canStartCheck;

    [SerializeField] private float _minimalRecoilSecondes = 0.1f;

    public Vector2 Velocity {get; private set; }
    public Vector2 MaxVelocity { get; private set; }
    private Vector2 previousVelocity;

    public Vector2 Pos {get; private set; }
    public Vector2 MaxPos { get; private set; }

    public Vector3 Rot { get; private set; }
    public Vector3 MaxRot { get; private set; }

    private void Start()
    {
        controller = GetComponentInParent<XRBaseController>();
        previousPosition = new(controller.transform.position.y, controller.transform.position.z);
        MaxVelocity = new();
        previousVelocity = Vector2.zero;
        MaxPos = new();
        MaxRot = new();
        InRecoil = false;

        EventSystem.Events.OnShoot += StartRecoil;
        EventSystem.Events.OnRecoilEnd += EndRecoil;
    }


    private void Update()
    {
        if (!InRecoil) return;
        CalculateVelocity();
        CalculatingMaxPosRot();

        if(previousVelocity == Vector2.zero)
        {
            previousVelocity = Velocity;
            return;
        }

        if (!_canStartCheck) return;
        if(Mathf.Sign(Velocity.x) != Mathf.Sign(previousVelocity.x) && Mathf.Sign(Velocity.y) != Mathf.Sign(previousVelocity.y))
            EventSystem.Events.TriggerRecoilEnd();
    }

    private void CalculateVelocity()
    {
        Vector2 currentPosition = new(controller.transform.position.y, controller.transform.position.z);
        Velocity = (currentPosition - previousPosition) / Time.deltaTime;
        MaxVelocity = Vector2.Max(MaxVelocity, Velocity);

        previousPosition = currentPosition;
    }

    private void CalculatingMaxPosRot()
    {
        var controllerTrns = controller.transform;
        MaxPos = Vector2.Max(MaxPos, new(controllerTrns.position.y, controllerTrns.position.z));
        MaxRot = Vector2.Max(MaxRot, controllerTrns.rotation.eulerAngles);
    }

    public void StartRecoil()
    {
        previousVelocity = Vector2.zero;
        Velocity = Vector2.zero;
        MaxVelocity = Vector2.zero;

        InRecoil = true;
        Invoke(nameof(StartCheck), _minimalRecoilSecondes);

        var controllerTrns = controller.transform;
        Pos = new(controllerTrns.position.y, controllerTrns.position.z);
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
        Pos = new(controllerTrns.position.y - Pos.x, controllerTrns.position.z - Pos.y);
        Rot = controllerTrns.rotation.eulerAngles - Rot;
    }

    void OnDestroy()
    {
        EventSystem.Events.OnShoot -= StartRecoil;
        EventSystem.Events.OnRecoilEnd -= EndRecoil;
    }
}
 