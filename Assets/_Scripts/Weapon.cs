using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    //temp
    public bool EndRecoilAtTriggerRelease = true;

    [SerializeField] private float _fireRate = 0.3f; 
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private ParticleSystem _muzzleFlashVFX;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private float _hapticDuration = 0.2f;
    [SerializeField] private float _hapticAmplitude = 1f;
    private AudioSource audioSource;
    private RecoilPerformance _recoil;

    [SerializeField] private InputActionReference _shootInputAction;


    private float nextFireTime = 0;

    private PoolSystem _bulletPool;
    private XRBaseController _controller;



    private void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;

        audioSource = GetComponent<AudioSource>();
        _recoil = GetComponent<RecoilPerformance>();
        _controller = GetComponentInParent<XRBaseController>();
    }

    private void OnEnable()
    {
        _shootInputAction.action.Enable();
        _shootInputAction.action.started += Shoot;
        _shootInputAction.action.canceled += EndShooting ;
    }

    private void OnDisable()
    {
        _shootInputAction.action.started -= Shoot;
        _shootInputAction.action.canceled -= EndShooting;
        _shootInputAction.action.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + _fireRate;

        EventSystem.Events.TriggerOnShoot(transform.parent.name);

        var bullet = _bulletPool.Get(_bulletSpawn.position, _bulletSpawn.rotation);

        _muzzleFlashVFX.Play(true);
        audioSource.PlayOneShot(_shootSound);
        _controller.SendHapticImpulse(_hapticAmplitude, _hapticDuration);
        _bulletPool.Return(bullet, 5.0f);
    }

    private void EndShooting(InputAction.CallbackContext context)
    {
        if (EndRecoilAtTriggerRelease)
            EventSystem.Events.TriggerRecoilEnd(transform.parent.name);
    }

    
}
