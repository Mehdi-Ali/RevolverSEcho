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



    void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;
        _bulletSplashPool = PoolManager.PoolInst.BulletSplash;
    }

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidB.velocity = transform.forward * _bulletSpeed;
    }

    public void ResetInst()
    {
        rigidB.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        EnableSplash(collision.GetContact(0).point);
        _bulletPool.Return(gameObject);
    }


    private void EnableSplash(Vector3 splashPosition)
    {
        var splash = _bulletSplashPool.Get(splashPosition, transform.rotation);
        _bulletSplashPool.Return(splash, 7.5f);
    }
}
