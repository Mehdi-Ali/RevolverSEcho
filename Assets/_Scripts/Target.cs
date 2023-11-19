using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100f;
    [SerializeField] float _damagePopupLifeTimeInSec= 2f;
    private PoolSystem _PopupPool;


    void Start()
    {
        _PopupPool = PoolManager.PoolInst.DamagePopup;
    }

    public void TakeDamage(float damage, bool isEcho = false)
    {
        _health -= damage;
        _health = Math.Max(_health, 0);

        DisplayDamage(damage);
        if (_health <= 0)
            Die();
    }

    private void DisplayDamage(float damage, bool isEcho = false)
    {
        Quaternion textInfoAsQuaternion;

        // we can make the damage types indexed as int and passed here and make a switch case
        if (isEcho)
            textInfoAsQuaternion = new Quaternion(damage, 1f, 0f, 0f);
        else
            textInfoAsQuaternion = new Quaternion(damage, 0f, 0f, 0f);

        var damagePopup = _PopupPool.Get(transform.position, textInfoAsQuaternion);
        _PopupPool.Return(damagePopup, _damagePopupLifeTimeInSec);
    }

    public void Die()
    {
        // TODO add a pool system for targets
        Destroy(this.gameObject);
    }
}

public interface IDamageable
{
    public void TakeDamage(float damage, bool isEcho = false);
    public void Die();
}