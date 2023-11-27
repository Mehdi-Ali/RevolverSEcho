using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPhysics : MonoBehaviour
{
    [SerializeField] private Transform _rest;
    [SerializeField] private float _stabilizationTime = 1.5f;
    private Rigidbody _rb;
    private bool _stopStabilization;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void TookDamage(float damage)
    {
        _stopStabilization = true;
        StartCoroutine(Stabilize(damage / 20f));
    }

    [Button]
    private IEnumerator Stabilize(float seconds = 0f)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(StabilizeOverTime(_stabilizationTime));
    }

    IEnumerator StabilizeOverTime(float duration)
    {
        _stopStabilization = false;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;

            // _rb.position = Vector3.Lerp(_rb.position, _rest.transform.position, progress);
            _rb.rotation = Quaternion.Lerp(_rb.rotation, Quaternion.identity, progress);

            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, progress);
            _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, progress);

            if (_stopStabilization)
                break;
            
            yield return null;
        }
    }
}
