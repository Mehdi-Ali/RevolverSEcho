using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Events { get; private set; }

    public event Action<string> OnShoot;
    public event Action<string> OnRecoilEnd;

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

    public void TriggerOnShoot(string controllerName)
    {
        OnShoot?.Invoke(controllerName);
    }

    public void TriggerRecoilEnd(string controllerName)
    {
        OnRecoilEnd?.Invoke(controllerName);
    }
}
