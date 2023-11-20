using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Events { get; private set; }

    public event Action<string, int> OnShoot;
    public event Action<string> OnRecoilEnd;
    public event Action<int, IDamageable> OnBulletHit;
    public event Action<string, float, int> OnEvolutionEnd;
    public event Action<string, float> OnEchoChargeChanged;

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

    public void TriggerRecoilEnd(string controllerName)
    {
        OnRecoilEnd?.Invoke(controllerName);
    }

    public void TriggerBulletHit(int bulletID, IDamageable target)
    {
        OnBulletHit?.Invoke(bulletID, target);
    }

    public void TriggerEvolutionEnd(string controllerName, float Score, int bulletID)
    {
        OnEvolutionEnd?.Invoke(controllerName, Score, bulletID);
    }

    public void TriggerEchoChargeChanged(string controllerName, float fillAmount)
    {
        OnEchoChargeChanged?.Invoke(controllerName, fillAmount);
    }
}
