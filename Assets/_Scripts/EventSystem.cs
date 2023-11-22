using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Events { get; private set; }

    public event Action<string, int> OnShoot;
    public event Action<string> OnRecoilEnd;
    public event Action<IDamageable, Vector3, int> OnBulletHit;
    public event Action<string, float, int> OnEvolutionEnd;
    public event Action<string, float> OnEchoChargeChanged;
    public event Action<string> OnEchoManagerStart;

    private void Awake()
    {
        if (Events == null)
        {
            Events = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerOnShoot(string controllerName, int bulletID)
    {
        OnShoot?.Invoke(controllerName, bulletID);
    }

    public void TriggerOnRecoilEnd(string controllerName)
    {
        OnRecoilEnd?.Invoke(controllerName);
    }

    public void TriggerOnBulletHit(int bulletID, IDamageable target, Vector3 contactPoint)
    {
        OnBulletHit?.Invoke(target, contactPoint, bulletID);
    }

    public void TriggerOnEvolutionEnd(string controllerName, float Score, int bulletID)
    {
        OnEvolutionEnd?.Invoke(controllerName, Score, bulletID);
    }

    public void TriggerOnEchoChargeChanged(string controllerName, float fillAmount)
    {
        OnEchoChargeChanged?.Invoke(controllerName, fillAmount);
    }


    public void TriggerOnEchoManagerStart(string controllerName)
    {
        OnEchoManagerStart?.Invoke(controllerName);
    }
}
