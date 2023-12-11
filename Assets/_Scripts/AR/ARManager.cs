using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.OpenXR.Features.Meta;

[RequireComponent(typeof(ARSession))]
public class ARManager : MonoBehaviour
{
    [SerializeField] private float _wavesTimerInSeconds = 3f;
    private ARSession _arSession;
    private ARPlaneManager _arPlaneManager;

    private float _timer = 0f;
    private List<ARPlane> _arPlanes;


    public void Start()
    {
        _arSession = GetComponent<ARSession>();
        _arPlaneManager = FindObjectOfType<ARPlaneManager>(true);
        _arPlaneManager.planesChanged += OnPlanesChanged;

        StartCoroutine(StartARSession());
        _arPlanes = new();

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
            var validSceneCapture = (_arSession.subsystem as MetaOpenXRSessionSubsystem).TryRequestSceneCapture();
        }
    }

    private bool GetARPlanes()
    {
        if (!_arPlaneManager)
            return false;

        UniversalMessage.Message.SendText($"{_arPlaneManager.trackables.count}");


        foreach (var item in _arPlaneManager.trackables)
        {
            _arPlanes.Add(item as ARPlane);
        }

        return _arPlanes.Count != 0;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        SpawnDroneOnPlanes(args.added, PlaneAlignment.HorizontalUp);
        foreach (var plane in args.added)
        {
            plane.GetComponent<MeshRenderer>().enabled = false;
            //or 
            // 
        }
    }

    public void SpawnNextWave()
    {
        if (GetARPlanes())
            SpawnDroneOnPlanes(_arPlanes, PlaneAlignment.HorizontalUp);
    }

    private void SpawnDroneOnPlanes(List<ARPlane> planesList, PlaneAlignment planesAlignment)
    {
        if (planesList.Count > 0)
        {
            foreach (var plane in planesList)
            {
                //UniversalMessage.Message.SendText($"{plane} , {plane.classification}");
                if (plane.alignment != planesAlignment)
                    continue;

                plane.transform.GetPositionAndRotation(out var position, out var Rotation);
                position.y += 0.25f;
                PoolManager.PoolInst.TargetCan.Get(position, Rotation);

            }

        }
    }

    void OnDestroy()
    {
        _arPlaneManager.planesChanged -= OnPlanesChanged;
    }
}
