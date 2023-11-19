using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    [SerializeField] private float _echoDamage;
    public float EchoCharge;
    private string _controllerName;

    private List<BulletData> _landedBullets;
    private Dictionary<int, float> _pendingEvaluations;


    void OnEnable()
    {
        EventSystem.Events.OnBulletHit += SaveHitBulletData;
        EventSystem.Events.OnEvolutionEnd += EvaluationEnd;
    }
    
    void Start()
    {
        _controllerName = transform.parent.name;
        _landedBullets = new();
        _pendingEvaluations = new Dictionary<int, float>();
    }

    private void SaveHitBulletData(int bulletID, IDamageable target)
    {
        BulletData bulletData = new(bulletID, target);
        _landedBullets.Add(bulletData);
        StartCoroutine(RemoveBullet(bulletData, 2f));

        if (_pendingEvaluations.TryGetValue(bulletID, out var score))
        {
            ApplyEvaluation(score, bulletData);
            _pendingEvaluations.Remove(bulletID);
        }
    }

    private IEnumerator RemoveBullet(BulletData bulletData, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_landedBullets.Contains(bulletData))
            _landedBullets.Remove(bulletData);
    }

    private void EvaluationEnd(string controllerName, float score, int bulletID)
    {
        if (controllerName != _controllerName)
            return;
        
        if (score == 0)
            return;

        var bullet = _landedBullets.Find(bullet => bullet._bulletID == bulletID);
        if (bullet._bulletTarget == null)
        {
            _pendingEvaluations.Add(bulletID, score);
            StartCoroutine(RemovePendingEvaluation(bulletID, 2f));
            return;
        }

        ApplyEvaluation(score, bullet);
    }

    private void ApplyEvaluation(float score, BulletData bullet)
    {
        _landedBullets.Remove(bullet);
        var damage = score * _echoDamage;
        EchoCharge += score;
        bullet._bulletTarget?.TakeDamage(damage, true);
    }

    private IEnumerator RemovePendingEvaluation(int bulletID, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_pendingEvaluations.ContainsKey(bulletID))
            _pendingEvaluations.Remove(bulletID);
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
