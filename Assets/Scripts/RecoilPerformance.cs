using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecoilPerformance : MonoBehaviour
{
    private XRBaseController controller;
    private Vector2 previousPosition;
    private Stopwatch recoilStopwatch;

    public Vector2 Velocity {get; private set; }
    public Vector2 MaxVelocity { get; private set; }

    private void Start()
    {
        recoilStopwatch = new Stopwatch();
        controller = GetComponentInParent<XRBaseController>();
        previousPosition = new(controller.transform.position.y, controller.transform.position.z);
        MaxVelocity = new();
    }

    private void Update()
    {
        Vector2 currentPosition = new(controller.transform.position.y, controller.transform.position.z) ;
        Velocity = (currentPosition - previousPosition) / Time.deltaTime;
        MaxVelocity = Vector2.Max(MaxVelocity, Velocity);

        previousPosition = currentPosition;
    }

    public void StartRecoil()
    {
        recoilStopwatch.Start();
    }

    public void EndRecoil()
    {
        MaxVelocity = Vector2.zero;
        recoilStopwatch.Stop();
        UnityEngine.Debug.Log($"Recoil duration: {recoilStopwatch.Elapsed.TotalSeconds} seconds ----");
        recoilStopwatch.Reset();
    }
}
 