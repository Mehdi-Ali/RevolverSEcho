using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingState : BaseState
{

    [SerializeField] private float _patrollingSpeed = 10f;
    [SerializeField] private float _maxRoamingPause = 5f;
    [SerializeField] private float _minRoamingPause = 1f;
    [SerializeField] private float _stopPatrollingDistance = 1f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _minRange = 5f;
    [SerializeField] private float _maxRange = 10.0f;
    [SerializeField] private float _transitionTime = 2f;
    [SerializeField] private float _droneMinimalAltitude = 0.50f; // may should getthose from the map it self 
    [SerializeField] private float _droneNormalAltitude = 3f; // may should getthose from the map it self 
    [SerializeField] private Transform _altitudeFree;




    private float _roamingPause;
    private float _timer;
    private float _startingAltitude;
    private float _altitudeToTravel;
    private bool _isInPause;
    private Vector3 _roamingPos;


    protected override void OnEnterState()
    {
        Enemy.NavAgent.speed = _patrollingSpeed;
        Enemy.NavAgent.angularSpeed = _rotationSpeed;
        _startingAltitude = _altitudeFree.localPosition.y;
        _roamingPause = Random.Range(_minRoamingPause, _maxRoamingPause);
        _timer = _roamingPause;
        _isInPause = false;
    }

    public override void OnUpdateState()
    {
        _timer += Time.deltaTime;

        if (_timer >= _roamingPause)
        {
            _roamingPos = GetRandomPosition(true);
            _startingAltitude = _altitudeFree.localPosition.y;
            _altitudeToTravel = _roamingPos.y - _startingAltitude;
            Enemy.NavAgent.SetDestination(_roamingPos);

            float transitionTime = 2 * Enemy.NavAgent.remainingDistance / Enemy.NavAgent.speed;
            StartCoroutine(AltitudeTransition(transitionTime));

            _timer = 0;
            _isInPause = false;
        }

        if (_isInPause)
            return;

        if (Vector3.Distance(GetDronePosition(), _roamingPos) <= _stopPatrollingDistance)
        {
            _isInPause = true;
            StopRoaming();
        }
    }

    private IEnumerator AltitudeTransition(float transitionTime)
    {
        if (transitionTime == 0)
            yield break;

        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime / transitionTime;
            progress = Mathf.Min(1, progress);

            var calculatedAltitude = Mathf.Lerp(_startingAltitude, _roamingPos.y, progress);

            _altitudeFree.localPosition = new Vector3(_altitudeFree.localPosition.x,
                                                        calculatedAltitude,
                                                        _altitudeFree.localPosition.z);

            yield return null;
        }
    }

    private Vector3 GetRandomPosition(bool inNavMeshBound = false)
    {
        var randomPosition = Enemy.transform.position + GetRandomDirection() * Random.Range(_minRange, _maxRange);
        if (randomPosition.y <= _droneMinimalAltitude)
            randomPosition.y = _droneNormalAltitude;

        if (! inNavMeshBound)
            return randomPosition;

        NavMeshHit hit;
        var areaMask = Enemy.NavAgent.areaMask;
        if (NavMesh.SamplePosition(randomPosition, out hit, _maxRange, areaMask))
        {
            var inBoundPosition = hit.position;
            inBoundPosition.y = randomPosition.y;
            return inBoundPosition;
        }
        else
        {
            return Enemy.transform.position;
        }
    }


    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void StopRoaming()
    {
        Enemy.NavAgent.SetDestination(GetDronePosition());

        // Enemy.TransitionToState(_enemy.IdleState);
        // in the idle 
        // Invoke(nameof(GoRoam), Random.Range(statics.MinRoamingPause , statics.MaxRoamingPause));
    }

    private Vector3 GetDronePosition()
    {
        var enemyTrans = Enemy.transform;
        return new Vector3(enemyTrans.position.x, _altitudeFree.transform.position.y, enemyTrans.position.z);
    }

    protected override void OnExitState()
    {
    }
}
