using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100f;
    private PoolSystem _PopupPool;
    private PoolSystem _VFXPool;



    void Start()
    {
        _PopupPool = PoolManager.PoolInst.DamagePopup;
        _VFXPool = PoolManager.PoolInst.EchoDamageVFX;
    }

    public void TakeDamage(float damage, Vector3 contactPoint,  int id = -1, bool isEcho = false)
    {
        if (this == null) return;

        _health -= damage;
        _health = Math.Max(_health, 0);

        if (!isEcho)
            DisplayNumbers(damage);
            
        else 
        {
            DisplayNumbers(damage, isEcho);
            StartEchoVFX(damage, contactPoint);
        }

        if (_health <= 0)
            Die();
    }


    private void DisplayNumbers(float damage, bool isEcho = false)
    {
        // we can make the damage types indexed as int and passed here and make a switch case
        Quaternion textInfoAsQuaternion;
        if (isEcho)
            textInfoAsQuaternion = new Quaternion(damage, 1f, 0f, 0f);

        else
            textInfoAsQuaternion = new Quaternion(damage, 0f, 0f, 0f);

        var damagePopup = _PopupPool.Get(transform.position, textInfoAsQuaternion);
    }

    private void StartEchoVFX(float damage, Vector3 contactPoint)
    {
        // maybe randomize rotation?
        var vfxInstance = _VFXPool.Get(contactPoint, transform.rotation);
        vfxInstance.transform.localScale = math.min((damage / 15f), 0.8f) * Vector3.one;
        _VFXPool.Return(vfxInstance, 2f);
    }

    public void Die()
    {
        // TODO add a pool system for targets
        Destroy(this.gameObject);
    }
}

public interface IDamageable
{
    public void TakeDamage(float damage, Vector3 contactPoint, int id = -1, bool isEcho = false);
    public void Die();
}