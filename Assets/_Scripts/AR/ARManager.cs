using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR.Features.Meta;

[RequireComponent(typeof(ARSession))]
public class ARManager : MonoBehaviour
{
    private ARSession _arSession;

    public void Start()
    {
        _arSession = GetComponent<ARSession>();
        StartCoroutine(StartARSession());
    }

    IEnumerator StartARSession()
    {
        bool shouldCheck = (ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability);

        if (shouldCheck)
            yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.Unsupported)
            Debug.Log("Device does not support AR");
            
        else
        {
            _arSession.enabled = true;
            var success = (_arSession.subsystem as MetaOpenXRSessionSubsystem).TryRequestSceneCapture();
        }
    }
}
