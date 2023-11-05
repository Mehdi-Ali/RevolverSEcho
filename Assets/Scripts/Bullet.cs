using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour, IPool
{
    [SerializeField] private Rigidbody rigidB;
    [SerializeField] private float _bulletSpeed = 100;


    private PoolSystem _bulletSplashPool;
    private PoolSystem _bulletPool;

    private Stopwatch shootingStopwatch;


    void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;
        _bulletSplashPool = PoolManager.PoolInst.BulletSplash;
        shootingStopwatch = new Stopwatch();
    }

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidB.velocity = transform.forward * _bulletSpeed;
        shootingStopwatch.Start();
    }

    public void ResetInst()
    {
        rigidB.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;

        EnableSplash(collision.GetContact(0).point);
        ShootingEnded();
        StartCoroutine(_bulletPool.Return(gameObject));
    }


    private void EnableSplash(Vector3 splashPosition)
    {
        var splash = _bulletSplashPool.Get(splashPosition, transform.rotation);
        StartCoroutine(_bulletSplashPool.Return(splash, 17.5f));
    }

    private void ShootingEnded()
    {
        shootingStopwatch.Stop();
        UnityEngine.Debug.Log($"shooting duration: {shootingStopwatch.Elapsed.TotalSeconds} seconds");
        shootingStopwatch.Reset();
    }
}
