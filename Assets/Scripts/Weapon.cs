using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    //temp
    public bool EndRecoilAtTriggerRelease = true;

    [SerializeField] private float _fireRate = 0.3f; 
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private ParticleSystem _muzzleFlashVFX;
    [SerializeField] private AudioClip _shootSound;
    private AudioSource audioSource;
    public RecoilPerformance Recoil;

    [SerializeField] private InputActionReference _shootInputAction;


    private float nextFireTime = 0;

    private PoolSystem _bulletPool;


    private void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;

        audioSource = GetComponent<AudioSource>();
        Recoil = GetComponent<RecoilPerformance>();
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

        var bullet = _bulletPool.Get(_bulletSpawn.position, _bulletSpawn.rotation);

        Recoil.StartRecoil();
        _muzzleFlashVFX.Play(true);
        audioSource.PlayOneShot(_shootSound);
        StartCoroutine(_bulletPool.Return(bullet, 5.0f));
    }

    private void EndShooting(InputAction.CallbackContext context)
    {
        if (EndRecoilAtTriggerRelease)
            Recoil.EndRecoil();
    }
}
