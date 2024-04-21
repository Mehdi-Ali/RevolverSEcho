using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "ScriptableObjects/Abilities/Dash", order = 2)]
public class DashAbilitySO : Ability
{
    [Space]
    [Header("Specific Stats")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDurationSec;


    //private 

    public override bool ActivateAbility(EchoManager echo)
    {
        if (!base.ActivateAbility(echo))
            return false;
        
        System.StartCoroutine(Dash());
        return true;
    }


    IEnumerator Dash()
    {
        Vector3 direction;
        var inputDirection = System.LeftThumbStick.action.ReadValue<Vector2>();

        if (inputDirection.magnitude == 0f)
            direction = System.MainCamera.transform.forward;
        else
        {
            var direction2D = inputDirection.normalized;
            float playerYRotation = System.MainCamera.transform.eulerAngles.y;
            direction = Quaternion.Euler(0, playerYRotation, 0) * new Vector3(direction2D.x, 0, direction2D.y);
        }

        float startTime = Time.time;
        while(Time.time < startTime + _dashDurationSec)
        {
            System.controller.SimpleMove(direction * _dashSpeed);
            yield return null;
        }
    }
}
