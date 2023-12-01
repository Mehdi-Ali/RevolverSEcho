using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public BaseState _currentState;
    [Space]
    public Idle IdleState;
    public Patrolling PatrollingState;
    public TakingDamage TakingDamageState;
    public Chasing ChasingState;
    public Attacking AttackingState;
    public Dying DyingState;
    [Space]
    public NavMeshAgent NavAgent;
    public Rigidbody RigidBody;
    public Transform Target;


    void Start()
    {
        TransitionToState(_currentState);
        NavAgent = GetComponent<NavMeshAgent>();
        RigidBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        _currentState.OnUpdateState();
    }
    void OnCollisionEnter(Collision _)
    {
        if (_currentState == IdleState)
            TookDamage(0.0f);
    }

    public void TookDamage(float damage)
    {
        TransitionToState(TakingDamageState);
        TakingDamageState.PrepareStabilization(damage);
    }

    public void TransitionToState(BaseState newState)
    {
        if (newState == _currentState || newState == null)
            return;
        
        if (_currentState != null)
            _currentState.EnterExit();

        _currentState = newState;
        _currentState.EnterState(this);
    }
}
