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

    protected override void OnEnterState()
    {
    }


    public void PrepareStabilization(float damage)
    {
        _stopStabilization = true;
        //SwitchControlsToRigidBody(true);
        StartCoroutine(StabilizeAfterSeconds(damage * _damageToDestabilizedTimeFactor / 20f));
    }

    private void SwitchControlsToRigidBody(bool isRigidBodyInControl)
    {
        // if (isRigidBodyInControl)
        //     _rb.position = _navAgent.nextPosition;
        // else
        //     _navAgent.nextPosition = _rb.position;

        // _navAgent.updatePosition = !isRigidBodyInControl;
        // _navAgent.updateRotation = !isRigidBodyInControl;
        // _rb.isKinematic = !isRigidBodyInControl;
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

            // the target will depend on the machine state ( player, 0, next point...)
            //var target = Quaternion.identity; // reset to 0.
            var target = Quaternion.LookRotation(Camera.main.transform.position - rigidBody.position);

            rigidBody.rotation = Quaternion.Lerp(rigidBody.rotation, target, progress);
            // _rb.position = Vector3.Lerp(_rb.position, _rest.transform.position, progress);


            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector3.zero, progress);
            rigidBody.angularVelocity = Vector3.Lerp(rigidBody.angularVelocity, Vector3.zero, progress);

            if (_stopStabilization)
                break;

            yield return null;
        }

        EndTakingDamage();
    }

    private void EndTakingDamage()
    {
        //SwitchControlsToRigidBody(false);
        Enemy.TransitionToState(Enemy.IdleState);
    }

    public override void OnUpdateState()
    {
    }

    protected override void OnExitState()
    {

    }
}