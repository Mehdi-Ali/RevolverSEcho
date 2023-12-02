using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    [SerializeField] private float _maxIdleDuration = 5f;
    [SerializeField] private float _minIdleDuration = 1f;

    private float _idlePause;

    protected override void OnEnterState()
    {
        _idlePause = Random.Range(_minIdleDuration, _maxIdleDuration);
    }

    public override void OnUpdateState()
    {
        if (Enemy == null)
            return;
        
        _idlePause -= Time.deltaTime;
        if (_idlePause <= 0)
        {
            Enemy.SwitchState(Enemy.PatrollingState);
        }
    }

    protected override void OnExitState()
    {

    }

}
