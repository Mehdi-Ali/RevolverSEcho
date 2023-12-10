using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.AI;

public class TakingDamage : BaseState
{
    [SerializeField] private float _stabilizationTime = 2f;
    [SerializeField] private float _damageToDestabilizedTimeFactor = 1f;
    private bool _stopStabilization;
    private Coroutine _currentStabilizeCoroutine;
    private float _timeToWait;

    public override void OnEnterState()
    {

    }


    public void PrepareStabilization(float damage)
    {
        _stopStabilization = true;
        SwitchControlsToRigidBody(true);
        StartCoroutine(StabilizeAfterSeconds(damage * _damageToDestabilizedTimeFactor / 20f));
    }

    private void SwitchControlsToRigidBody(bool isRigidBodyInControl)
    {
        var rigidBody = Enemy.RigidBody;
        var navAgent = Enemy.NavAgent;

        if (isRigidBodyInControl)
            rigidBody.position = navAgent.nextPosition;
        else
            navAgent.nextPosition = rigidBody.position;

        navAgent.updatePosition = !isRigidBodyInControl;
        navAgent.updateRotation = !isRigidBodyInControl;
        rigidBody.isKinematic = !isRigidBodyInControl;
    }

    private IEnumerator StabilizeAfterSeconds(float seconds = 0f)
    {
        _timeToWait += seconds;

        if (_currentStabilizeCoroutine != null)
            StopCoroutine(_currentStabilizeCoroutine);

        _currentStabilizeCoroutine = StartCoroutine(StabilizeAfterMoreSeconds());

        yield return _currentStabilizeCoroutine;
        _currentStabilizeCoroutine = null;
    }

    private IEnumerator StabilizeAfterMoreSeconds()
    {
        yield return new WaitForSeconds(_timeToWait);
        _timeToWait = 0;
        StartCoroutine(StabilizeOverTime(_stabilizationTime));
    }

    IEnumerator StabilizeOverTime(float duration)
    {
        if (Enemy == null)
            yield break;

        _stopStabilization = false;
        var rigidBody = Enemy.RigidBody;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            var target = Quaternion.LookRotation(Enemy.Target.position - rigidBody.position);

            rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, target, progress);

            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector3.zero, progress);
            rigidBody.angularVelocity = Vector3.Lerp(rigidBody.angularVelocity, Vector3.zero, progress);

            if (_stopStabilization)
                break;

            yield return null;
        }

        Enemy.SwitchState(Enemy.IdleState);
    }

    public override void OnUpdateState()
    {
    }

    public override void OnExitState()
    {
        SwitchControlsToRigidBody(false);
    }
}