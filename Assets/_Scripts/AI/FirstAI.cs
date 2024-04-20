using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAI : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody RigidBody;
    //[Space]


    void OnTriggerEnter(Collider other)
    {
        Dash();
    }

    void OnCollisionEnter(Collision other)
    {
        Dash();
    }

    private void Dash(Vector3? dashPosition = null)
    {
        if (dashPosition == null)
        {
            dashPosition = transform.position + //randoom postion
                new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        }

        this.transform.position = (Vector3)dashPosition;
    }
}
