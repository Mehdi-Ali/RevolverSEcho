using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private float _reticleSize = 0.25f;
    private GameObject _reticle;
    private Camera _mainCamera;
    private LineRenderer _lineRender;
    private string _controllerName;

    void OnEnable()
    {
        EventSystem.Events.OnShoot += DisableLaser;
        EventSystem.Events.OnRecoilEnd += EnableLaserAndReticle;
    }

    void Start()
    {
        _lineRender = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
        _controllerName = transform.parent.parent.name;

        // if (_reticle == null)
        GetReticle();

        EnableLaserAndReticle(_controllerName);
    }

    private void EnableLaserAndReticle(string controllerName)
    {
        if (controllerName != _controllerName)
            return;
        
        _lineRender.enabled = true;
        _reticle.SetActive(true);
    }

    private void DisableLaser(string controllerName, int _)
    {
        if (controllerName != _controllerName)
            return;
        
        _lineRender.enabled = false;
        _reticle.SetActive(false);
    }

    private void GetReticle()
    {
        if (_controllerName == "Right Controller")
            _reticle = Reticles.Instance.RightReticle;

        else if (_controllerName == "Left Controller")
            _reticle = Reticles.Instance.LeftReticle;
    }

    void LateUpdate()
    {
        _lineRender.SetPosition(0, transform.position);
        Vector3 endPoint;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance))
            endPoint = hit.point;

        else
            endPoint = transform.position + transform.forward * _maxDistance;


        _lineRender.SetPosition(1, endPoint);
        HandleReticle(endPoint);

    }

    private void HandleReticle(Vector3 endPoint)
    {
        var reticleTrans = _reticle.transform;
        var distance = math.max(Vector3.Distance(endPoint, _mainCamera.transform.position), 1.2f);
        var scale = _reticleSize * distance * (Vector3.up + Vector3.right) + Vector3.forward;

        reticleTrans.position = endPoint;
        reticleTrans.localScale = scale;
        reticleTrans.LookAt(_mainCamera.transform);
    }
    

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= DisableLaser;
        EventSystem.Events.OnRecoilEnd -= EnableLaserAndReticle;
    }
}
