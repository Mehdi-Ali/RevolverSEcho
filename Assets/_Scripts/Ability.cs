using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TemplateAbility", menuName = "ScriptableObjects/Abilities/TemplateAbility", order = 1)]
public class Ability : ScriptableObject
{
    [Header("General Stats")]
    [SerializeField] private int level;
    [SerializeField] [TextArea] private string Description;

    public AbilitiesSystem System;

    public virtual bool ActivateAbility(EchoManager echo)
    {
        if(!echo.ConsumeEcho(level))
        {
            Debug.Log("not enough charges, perform a better recoil!!!");
            return false;
        }

        return true;
    }

    public virtual void CancelAbility()
    {
        //Debug.Log("CancelAbility");
    }

}
