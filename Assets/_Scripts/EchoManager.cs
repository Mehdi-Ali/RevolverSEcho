using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    [SerializeField] private float _echoDamage;
    public float EchoCharge;
    private string _controllerName;

    private List<BulletData> _landedBullets;

    void OnEnable()
    {
        EventSystem.Events.OnBulletHit += SaveHitBulletData;
        EventSystem.Events.OnEvolutionEnd += EvaluationEnd;
    }
    
    void Start()
    {
        _controllerName = transform.parent.name;
        _landedBullets = new();
    }

    private void SaveHitBulletData(int bulletID, IDamageable target)
    {
        _landedBullets.Add(new(bulletID, target));
    }

    private void EvaluationEnd(string controllerName, float score, int bulletID)
    {
        if (controllerName != _controllerName)
            return;


        var bullet = _landedBullets.Find(bullet => bullet._bulletID == bulletID);
        if (bullet._bulletTarget == null)
            return;

        _landedBullets.Remove(bullet);
        var damage = score * _echoDamage;
        EchoCharge += score;
        bullet._bulletTarget.TakeDamage(damage);
        Debug.Log("Echo damage: " + damage);
    }



    void OnDisable()
    {
        EventSystem.Events.OnBulletHit -= SaveHitBulletData;
        EventSystem.Events.OnEvolutionEnd -= EvaluationEnd;

    }
}

public struct BulletData
{
    public int _bulletID;
    public IDamageable _bulletTarget;

    public BulletData(int bulletID, IDamageable bulletTarget)
    {
        _bulletID = bulletID;
        _bulletTarget = bulletTarget;
    }
}
