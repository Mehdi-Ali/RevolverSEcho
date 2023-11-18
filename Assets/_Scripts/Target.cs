using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100f;

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _health = Math.Max(_health, 0);

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        // TODO add a pool system for targets
        Destroy(this.gameObject);
    }
}

public interface IDamageable
{
    public void TakeDamage(float damage);
    public void Die();
}