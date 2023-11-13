using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private Image _reticle;
    private Camera _mainCamera;
    private LineRenderer _lineRender;

    void OnEnable()
    {
        EventSystem.Events.OnShoot += DisableLaser;
        EventSystem.Events.OnRecoilEnd += EnableLaserAndReticle;

        _lineRender = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;

        if (_reticle == null)
            GetReticle();

        EnableLaserAndReticle();
    }

    private void EnableLaserAndReticle(string _="")
    {
        _lineRender.enabled = true;
        _reticle.enabled = true;
    }

    private void DisableLaser(string obj)
    {
        _lineRender.enabled = false;
        _reticle.enabled = false;
    }

    private void GetReticle()
    {
        var controllerName = transform.parent.parent.name;

        if (controllerName == "Right Controller")
            _reticle = WorldUIElement.Elements.RightReticle;

        else if (controllerName == "Left Controller")
            _reticle = WorldUIElement.Elements.LeftReticle;
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
        _reticle.transform.position = _mainCamera.WorldToScreenPoint(endPoint);
    }

    void OnDisable()
    {
        EventSystem.Events.OnShoot -= DisableLaser;
        EventSystem.Events.OnRecoilEnd -= EnableLaserAndReticle;
    }
}
