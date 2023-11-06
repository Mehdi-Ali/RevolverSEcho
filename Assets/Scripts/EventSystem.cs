using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Events { get; private set; }

    public event Action OnShoot;
    public event Action OnRecoilEnd;

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

    public void TriggerOnShoot()
    {
        OnShoot?.Invoke();
    }

    public void TriggerRecoilEnd()
    {
        OnRecoilEnd?.Invoke();
    }
}
