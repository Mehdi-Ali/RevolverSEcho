using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecoilPerformance : MonoBehaviour
{
    private XRBaseController controller;
    private Vector2 previousPosition;
    private Vector2 velocity;

    private void Start()
    {
        controller = GetComponentInParent<XRBaseController>();
        previousPosition = new(controller.transform.position.y, controller.transform.position.z);
    }

    private void Update()
    {
        Vector2 currentPosition = new(controller.transform.position.y, controller.transform.position.z) ;
        velocity = (currentPosition - previousPosition) / Time.deltaTime;

        previousPosition = currentPosition;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }
}
 