using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrollingState", menuName = "ScriptableObjects/Enemies/States/PatrollingState", order = 1)]
public class PatrollingState : BaseState
{

    [SerializeField] private float _patrollingSpeed = 10f;
    [SerializeField] private float _maxRoamingPause = 5f;
    [SerializeField] private float _minRoamingPause = 1f;
    [SerializeField] private float _stopPatrollingDistance = 1f;




    private float _timer;
    private Vector3 _roamingPos;


    protected override void OnEnterState()
    {
        Enemy.NavAgent.speed = _patrollingSpeed;
        _timer = 0f;
    }

    public override void OnUpdateState()
    {
        _timer += Time.deltaTime;

        if (_timer > _maxRoamingPause)
        {
            _roamingPos = GetRandomPosition();
            _roamingPos.y = Enemy.transform.position.y;

            Enemy.NavAgent.SetDestination(_roamingPos);
            _timer = 0;
        }

        if (Vector3.Distance(Enemy.transform.position, _roamingPos) < _stopPatrollingDistance)
            StopRoaming();
    }

    private Vector3 GetRandomPosition()
    {
        return Enemy.transform.position + GetRandomDirection() * Random.Range(2.5f, 5.0f);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void StopRoaming()
    {
        // Enemy.TransitionToState(_enemy.IdleState);
        // _enemy.NavAgent.destination = transform.position;

        // in the idle 
        // Invoke(nameof(GoRoam), Random.Range(statics.MinRoamingPause , statics.MaxRoamingPause));
    }

    protected override void OnExitState()
    {
    }
}
