using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour, IPool
{
    [SerializeField] private Rigidbody rigidB;
    [SerializeField] private float _bulletSpeed = 100;
    [SerializeField] private float _bulletDamage = 20;

    private PoolSystem _bulletSplashPool;
    private PoolSystem _bulletPool;
    private int _id;


    void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;
        _bulletSplashPool = PoolManager.PoolInst.BulletSplash;
    }

    public void Initialize(int id, Vector3 position, Quaternion rotation)
    {
        _id = id; 
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

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(_bulletDamage);
            EventSystem.Events.TriggerBulletHit(_id, target);
        }

        _bulletPool.Return(gameObject);
    }


    private void EnableSplash(Vector3 splashPosition)
    {
        var splash = _bulletSplashPool.Get(splashPosition, transform.rotation);
        _bulletSplashPool.Return(splash, 7.5f);
    }
}
