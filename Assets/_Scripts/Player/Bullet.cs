using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour, IPool
{
    [SerializeField] private Rigidbody rigidB;

    // move to stat SO
    [SerializeField] private float _bulletSpeed = 100;
    [SerializeField] private float _bulletDamage = 20;
    [SerializeField] private GameObject trail;

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

        rigidB.isKinematic = false;
        rigidB.velocity = transform.forward * _bulletSpeed;

        trail.SetActive(true);
    }

    public void ResetInst()
    {
        if (rigidB.isKinematic == false)
        {
            rigidB.velocity = Vector3.zero;
            rigidB.isKinematic = true;
        }
        trail.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.activeSelf == true)
            return;

        var contactPoint = collision.GetContact(0).point;

        if (collision.gameObject.TryGetComponent<DamageableTarget>(out DamageableTarget target))
        {
            contactPoint = target.transform.InverseTransformPoint(contactPoint);
            target.TakeDamage(_bulletDamage, contactPoint, _id);
            EventSystem.Events.TriggerOnBulletHit(_id, target, contactPoint);
            EnableSplash(contactPoint, target.SplashType);

        }
        else
            EnableSplash(contactPoint);

        _bulletPool.Return(gameObject);
    }


    private void EnableSplash(Vector3 splashPosition, SplashType splashTyp = SplashType.Default)
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
                return;
        }
        // todo I need to make the splash parent to the object: (modify the GET to accept Transforms parents)
        GameObject splash = _bulletSplashPool.Get(splashPosition, transform.rotation);
        _bulletSplashPool.Return(splash, 7.5f);
    }
}
