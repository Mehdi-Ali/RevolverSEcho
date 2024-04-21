using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAI : DamageableTarget
{
    // [Header("Dependencies")]
    // public Rigidbody RigidBody;
    //[Space]

    public override void TakeDamage(float damage, Vector3 contactPoint, int id = -1, bool isEcho = false)
    {
        base.TakeDamage(damage, contactPoint, id, isEcho);
        Dash();
    }

    private void Dash(Vector3? dashPosition = null)
    {
        if (dashPosition == null)
        {
            Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0,
                                                 UnityEngine.Random.Range(-1f, 1f));

            dashPosition = transform.position + randomVector;
        }

        this.transform.position = (Vector3)dashPosition;
    }
}
