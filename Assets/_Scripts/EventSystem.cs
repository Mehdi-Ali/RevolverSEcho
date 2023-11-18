using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Events { get; private set; }

    public event Action<string, int> OnShoot;
    public event Action<string> OnRecoilEnd;
    public event Action<string, int> OnBulletHit;

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

    public void TriggerBulletHit(string controllerName, int bulletID)
    {
        OnBulletHit?.Invoke(controllerName, bulletID);
    }
}
