using System;
using System.Collections;
using System.Collections.Generic;
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

    void OnEnable()
    {
        EventSystem.Events.OnShoot += DisableLaser;
        EventSystem.Events.OnRecoilEnd += EnableLaserAndReticle;
    }

    void Start()
    {
        _lineRender = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;

        if (_reticle == null)
            GetReticle();

        EnableLaserAndReticle();
    }

    private void EnableLaserAndReticle(string _="")
    {
        _lineRender.enabled = true;
        _reticle.SetActive(true);
    }

    private void DisableLaser(string obj)
    {
        _lineRender.enabled = false;
        _reticle.SetActive(false);
    }

    private void GetReticle()
    {
        var controllerName = transform.parent.parent.name;

        if (controllerName == "Right Controller")
            _reticle = Reticles.Instance.RightReticle;

        else if (controllerName == "Left Controller")
            _reticle = Reticles.Instance.LeftReticle;
    }

    void Update()
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
        var distance = Vector3.Distance(endPoint, _mainCamera.transform.position);
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
