using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    public UnityEvent OnTrigger;
    public UnityEvent OnCollision;

    void Start()
    {
        if (OnTrigger == null)
            OnTrigger = new UnityEvent();
            
        if (OnCollision == null)
            OnCollision = new UnityEvent();
    }

    void OnTriggerEnter(Collider other)
    {
        UniversalMessage.Message.SendText("OnTrigger");
        OnTrigger?.Invoke();
    }

    void OnCollisionEnter(Collision other)
    {
        UniversalMessage.Message.SendText("OnCollision");
        OnCollision?.Invoke();
    }
}