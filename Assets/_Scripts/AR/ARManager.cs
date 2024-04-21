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

        foreach (var item in _arPlaneManager.trackables)
        {
            _arPlanes.Add(item as ARPlane);
        }

        return _arPlanes.Count != 0;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        _arPlanes = args.added;
        SpawnCans();
        SpawnDrones();
    }



    public void SpawnNextWave()
    {
        if (!GetARPlanes())
            return;

        SpawnCans();
        SpawnDrones();
    }

    private void SpawnCans()
    {
        SpawnEnemies(PoolManager.PoolInst.TargetCan, PlaneAlignment.HorizontalUp, 0.25f);
    }

    private void SpawnDrones()
    {
        if (RandomBoolean(0.85f))
            SpawnEnemies(PoolManager.PoolInst.TargetDrone, PlaneAlignment.HorizontalUp, 1f);
    }

    private void SpawnEnemies(PoolInstance poolSystem, PlaneAlignment planesAlignment, float yOffset)
    {
        if (_arPlanes.Count > 0)
        {
            foreach (var plane in _arPlanes)
            {
                if (plane.alignment != planesAlignment)
                    continue;

                plane.transform.GetPositionAndRotation(out var position, out var Rotation);
                position.y += yOffset;
                poolSystem.Get(position, Rotation);

            }

        }
    }

    public bool RandomBoolean(float weight = 0.5f)
    {
        return UnityEngine.Random.value < weight;
    }

    void OnDestroy()
    {
        _arPlaneManager.planesChanged -= OnPlanesChanged;
    }
}
