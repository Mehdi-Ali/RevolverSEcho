using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Bullet : MonoBehaviour, IPool
{
    [SerializeField] private Rigidbody rigidB;

    // move to stat SO
    [SerializeField] private float _bulletSpeed = 100;
    [SerializeField] private float _bulletDamage = 20;
    [SerializeField] private GameObject trail;

    private PoolInstance _bulletSplashPool;
    private PoolInstance _bulletPool;
    private int _id;

    // Pool Settings
    [SerializeField] private bool _dynamicParent = true;
    public bool DynamicParent { get => _dynamicParent; set => _dynamicParent = value; }

    void Start()
    {
        _bulletPool = PoolManager.PoolInst.Bullet;
        _bulletSplashPool = PoolManager.PoolInst.BulletSplash;
    }

    public void Initialize(int id, Vector3 position, Quaternion rotation, Vector3 scale, float delay = 0f)
    {
        _id = id;
        transform.position = position;
        transform.rotation = rotation;

        rigidB.isKinematic = false;
        rigidB.velocity = transform.forward * _bulletSpeed;

        trail.SetActive(true);
    }

    public void ResetInst()
    {
        trail.SetActive(false);

        if (rigidB.isKinematic == false)
        {
            rigidB.velocity = Vector3.zero;
            rigidB.isKinematic = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 contactPoint = collision.GetContact(0).point;
        Transform hitObject = collision.gameObject.transform;

        if (collision.gameObject.TryGetComponent<DamageableTarget>(out DamageableTarget target))
        {
            target.TakeDamage(_bulletDamage, contactPoint, _id);
            Transform contactPointTransform = SpawnSplash(contactPoint, hitObject, target.SplashType);
            EventSystem.Events.TriggerOnBulletHit(_id, target, contactPointTransform);
        }
        else
            SpawnSplash(contactPoint, hitObject);

        _bulletPool.ReturnTomPool(gameObject);
    }


    private Transform SpawnSplash(Vector3 splashPosition, Transform hitObject, SplashType splashTyp = SplashType.Default)
    {
        switch (splashTyp)
        {
            case SplashType.Default:
                break;
            case SplashType.Metal:
                break;
            case SplashType.Wood:
                break;
            case SplashType.ElectronicEnemy:
                break;
            case SplashType.OrganicEnemy:
                break;
            case SplashType.NoSplash:
                return null;
        }

        GameObject splash = _bulletSplashPool.SpawnFromPool(splashPosition, transform.rotation, hitObject);
        _bulletSplashPool.ReturnTomPool(splash, 7.5f);
        return splash.transform;
    }
}
