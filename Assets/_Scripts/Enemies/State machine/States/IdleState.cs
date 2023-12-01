using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    [SerializeField] Animator _animator;


    protected override void OnEnterState()
    {
        _animator.enabled = true;

    }

    public override void OnUpdateState()
    {

    }

    protected override void OnExitState()
    {
        _animator.enabled = false;

    }

}
