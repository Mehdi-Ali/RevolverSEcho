using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
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

    [SerializeField] private BaseState _currentState;

    void Start()
    {
        SwitchState(IdleState);
        NavAgent = GetComponent<NavMeshAgent>();
        RigidBody = GetComponent<Rigidbody>();
        Target = this.transform;
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
        SwitchState(TakingDamageState);
        TakingDamageState.PrepareStabilization(damage);
    }

    void Attack(Player target)
    {
        Target = target.transform;
        SwitchState(AttackingState);

    }

    public void SwitchState(BaseState newState)
    {
        if (newState == _currentState || newState == null)
            return;
        
        if (_currentState != null)
            _currentState.EnterExit();

        _currentState = newState;
        _currentState.EnterState(this);
    }
}
