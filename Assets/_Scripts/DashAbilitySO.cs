using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "ScriptableObjects/Abilities/Dash", order = 1)]
public class DashAbilitySO : Ability
{
    [Space]
    [Header("Specific Stats")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDurationSec;

    public override bool ActivateAbility(EchoManager echo)
    {
        if (!base.ActivateAbility(echo))
            return false;
        
        System.StartCoroutine(Dash());
        return true;
    }


    IEnumerator Dash()
    {
        var direction = System.MainCamera.transform.forward;
        float startTime = Time.time;

        while(Time.time < startTime + _dashDurationSec)
        {
            System.controller.SimpleMove(direction * _dashSpeed);
            yield return null;
        }
    }
}
