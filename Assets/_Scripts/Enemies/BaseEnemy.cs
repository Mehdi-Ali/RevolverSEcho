using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected BaseState _currentState;
    [SerializeField] protected BaseState[] _states;
    public NavMeshAgent NavAgent;


    void Start()
    {
        InitializeCurrentStateOrFirstInStates();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    private void InitializeCurrentStateOrFirstInStates()
    {
        var state = _states[0];
        if (_currentState != null)
            state = _currentState;

        TransitionToState(state);
    }

    void Update()
    {
        _currentState.OnUpdateState();
    }

    public void TransitionToState(BaseState newState)
    {
        if (_currentState != null)
            _currentState.EnterExit();

        if (newState == null)
            return;

        _currentState = newState;
        _currentState.EnterState(this);
    }
}
