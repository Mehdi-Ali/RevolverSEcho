using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed = 760f;
    [SerializeField] private float _fireRate = 0.5f; 
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private ParticleSystem _muzzleFlashVFX;
    [SerializeField] private AudioClip _shootSound;
    private AudioSource audioSource;

    [SerializeField] private InputActionReference _shootInputAction;

    private Stopwatch recoilStopwatch;

    private float nextFireTime = 0;

    private void Start()
    {
        recoilStopwatch = new Stopwatch();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _shootInputAction.action.Enable();
        _shootInputAction.action.started += Shoot;
        _shootInputAction.action.canceled += RecoilEnded ;
    }

    private void OnDisable()
    {
        _shootInputAction.action.started -= Shoot;
        _shootInputAction.action.canceled -= RecoilEnded;
        _shootInputAction.action.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + _fireRate;

        var bulletGameObject = _bulletPrefab.gameObject;
        var bullet = (GameObject)Instantiate(
            bulletGameObject,
            _bulletSpawn.position,
            _bulletSpawn.rotation);

        audioSource.PlayOneShot(_shootSound);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _bulletSpeed;
        _muzzleFlashVFX.Play(true);
        recoilStopwatch.Start();
        Destroy(bullet, 5.0f);
    }


    private void RecoilEnded(InputAction.CallbackContext context)
    {
        recoilStopwatch.Stop();
        UnityEngine.Debug.Log($"Recoil duration: {recoilStopwatch.Elapsed.TotalSeconds} seconds ----");
        recoilStopwatch.Reset();
    }

}
