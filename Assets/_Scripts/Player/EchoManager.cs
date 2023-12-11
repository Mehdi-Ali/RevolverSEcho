using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EchoManager : MonoBehaviour
{
    [SerializeField] private float _echoDamage;
    [SerializeField] private float _initialEchoCharge = 3f;
    [SerializeField] private float _echoCharge;
    [SerializeField] private int _maxEchoCharge = 10;
    [SerializeField] private float _echoForceFactor = 5f;
    public string ControllerName;

    private List<BulletData> _landedBullets;
    private Dictionary<int, float> _pendingEvaluations;

    void OnEnable()
    {
        EventSystem.Events.OnBulletHit += SaveHitBulletData;
        EventSystem.Events.OnEvolutionEnd += EvaluationEnd;
    }
    
    void Start()
    {
        ControllerName = transform.parent.name;
        _landedBullets = new();
        _pendingEvaluations = new Dictionary<int, float>();

        _echoCharge = _initialEchoCharge;
        EventSystem.Events.TriggerOnEchoChargeChanged(ControllerName, _echoCharge/_maxEchoCharge);

        EventSystem.Events.TriggerOnEchoManagerStart(ControllerName);
    }

    private void SaveHitBulletData(DamageableTarget target, Vector3 contactPoint, int bulletID)
    {
        BulletData bulletData = new(bulletID, target, contactPoint);
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
        if (controllerName != ControllerName)
            return;
        
        var bullet = _landedBullets.Find(bullet => bullet.ID == bulletID);
        if (bullet.Target == null)
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
        ChargeEcho(score);

        if (!bullet.Target)
            return;

        bullet.Target.TakeDamage(damage, bullet.ContactPoint, bullet.ID, true);
        bullet.Target.ApplyForce(damage * _echoForceFactor, bullet.ContactPoint);
    }

    private void ChargeEcho(float score)
    {
        _echoCharge += score;
        _echoCharge = Math.Min(_echoCharge, _maxEchoCharge);
        var fillAmount = _echoCharge / _maxEchoCharge;

        EventSystem.Events.TriggerOnEchoChargeChanged(ControllerName, fillAmount);
    }

    public bool ConsumeEcho(float consumedCharges)
    {
        var newEchoCharge = _echoCharge - consumedCharges;
        if (newEchoCharge < 0)
        {
            UniversalMessage.Message.SendText($"You don't have enough Echo");
            return false;

        }

        _echoCharge = newEchoCharge;
        
        var fillAmount = _echoCharge / _maxEchoCharge;
        EventSystem.Events.TriggerOnEchoChargeChanged(ControllerName, fillAmount);

        return true;
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
    public int ID;
    public DamageableTarget Target;
    public Vector3 ContactPoint;

    public BulletData(int bulletID, DamageableTarget bulletTarget, Vector3 bulletContactPoint)
    {
        ID = bulletID;
        Target = bulletTarget;
        ContactPoint = bulletContactPoint;
    }
}
