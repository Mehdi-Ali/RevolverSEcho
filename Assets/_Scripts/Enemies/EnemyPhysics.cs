using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPhysics : MonoBehaviour
{
    [SerializeField] private float _stabilizationTime = 1.5f;
    [SerializeField] private float _damageToDestabilizedTimeFactor = 1f;
    private Rigidbody _rb;
    private NavMeshAgent _navAgent;
    private bool _stopStabilization;
    private Coroutine _currentStabilizeCoroutine;
    private float _timeToWait;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navAgent = GetComponent<NavMeshAgent>();
        _timeToWait += 0.0f;

    }


    public void TookDamage(float damage)
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

    [Button]
    private IEnumerator StabilizeAfterSeconds(float seconds = 0f)
    {
        _timeToWait += seconds;
        
        if (_currentStabilizeCoroutine != null)
            StopCoroutine(_currentStabilizeCoroutine);

        _currentStabilizeCoroutine = StartCoroutine(WaitAndStabilize());

        yield return _currentStabilizeCoroutine;
        _currentStabilizeCoroutine = null;
    }

    private IEnumerator WaitAndStabilize()
    {
        yield return new WaitForSeconds(_timeToWait);
        _timeToWait = 0;
        StartCoroutine(StabilizeOverTime(_stabilizationTime));
    }

    IEnumerator StabilizeOverTime(float duration)
    {
        _stopStabilization = false;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;

            // the target will depend on the machine state ( player, 0, next point...)
            //var target = Quaternion.identity; // reset to 0.
            var target = Quaternion.LookRotation(Camera.main.transform.position - _rb.position);

            _rb.rotation = Quaternion.Lerp(_rb.rotation, target, progress);
            // _rb.position = Vector3.Lerp(_rb.position, _rest.transform.position, progress);


            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, progress);
            _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, progress);

            if (_stopStabilization)
            {
                // SwitchControlsToRigidBody(false);
                break;
            }
            
            yield return null;
        }

        // SwitchControlsToRigidBody(false);
    }
}
